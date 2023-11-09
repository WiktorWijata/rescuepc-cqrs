using System.Collections.Generic;
using RescuePC.Software.CQRS.Event;

namespace RescuePC.Software.CQRS;

public interface IAggregateRoot
{
    void RaiseEvent<TEvent>(TEvent @event) where TEvent : IEvent;

    Queue<IEvent> GetEventsToPublish();
}
