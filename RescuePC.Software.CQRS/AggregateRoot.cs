using System.Collections.Generic;
using RescuePC.Software.CQRS.Event;

namespace RescuePC.Software.CQRS;

public abstract class AggregateRoot<TKey> : IAggregateRoot
{
    public abstract TKey Id { get; protected set; }

    private Queue<IEvent> _eventsQueue = new Queue<IEvent>();

    public virtual void RaiseEvent<TEvent>(TEvent @event) where TEvent : IEvent
    {
        if (_eventsQueue == null)
        {
            _eventsQueue = new Queue<IEvent>();
        }

        _eventsQueue.Enqueue(@event);
    }

    public virtual Queue<IEvent> GetEventsToPublish()
    {
        if (_eventsQueue == null)
        {
            _eventsQueue = new Queue<IEvent>();
        }

        return _eventsQueue;
    }
}

public abstract class AggregateRoot : AggregateRoot<long> { }
