using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RescuePC.Software.CQRS.Event;

public class EventBus : IEventBus
{
    private readonly Func<Type, IEnumerable<IHandleEvent>> _handlersFactory;

    public EventBus(Func<Type, IEnumerable<IHandleEvent>> handlersFactory)
    {
        _handlersFactory = handlersFactory;
    }

    public async ValueTask Publish<TEvent>(TEvent @event) where TEvent : IEvent
    {
        var handlers = _handlersFactory(@event.GetType()).Cast<dynamic>();

        foreach (var handler in handlers)
        {
            await handler.Handle(@event);
        }
    }
}
