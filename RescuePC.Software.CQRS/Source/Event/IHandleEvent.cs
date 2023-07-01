namespace RescuePC.Software.CQRS.Source.Event;

public interface IHandleEvent { }

public interface IHandleEvent<in TEvent> : IHandleEvent where TEvent : IEvent
{
    ValueTask Handle(TEvent @event);
}