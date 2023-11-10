using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RescuePC.Software.CQRS.Event;

namespace RescuePC.Software.CQRS;

public class PublishEventsInterceptor : SaveChangesInterceptor
{
    private readonly IEventBus _eventBus;

    public PublishEventsInterceptor(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        PublishEntitiesEvents(eventData).ConfigureAwait(false).GetAwaiter().GetResult();
        return base.SavedChanges(eventData, result);
    }

    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        await PublishEntitiesEvents(eventData);
        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    private async Task PublishEntitiesEvents(DbContextEventData eventData)
    {
        var entries = eventData.Context?.ChangeTracker.Entries();
        if (entries != null)
        {
            foreach (var entry in entries.ToArray())
            {
                var entity = entry.Entity;
                if (!(entity is IAggregateRoot aggregateRoot))
                {
                    continue;
                }

                var originalQueue = aggregateRoot.GetEventsToPublish();
                var events = new Queue<IEvent>(originalQueue);
                originalQueue.Clear();
                while (events.Count > 0)
                {
                    var @event = events.Dequeue();
                    await _eventBus.Publish(@event).ConfigureAwait(false);
                }
            }
        }
    }
}
