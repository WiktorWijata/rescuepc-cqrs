using System.Threading.Tasks;

namespace RescuePC.Software.CQRS.Event;

public interface IHandleEvent { }

public interface IHandleEvent<in TEvent> : IHandleEvent where TEvent : IEvent
{
    ValueTask Handle(TEvent @event);
}