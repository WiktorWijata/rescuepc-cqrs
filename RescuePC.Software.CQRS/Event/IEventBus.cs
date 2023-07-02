using System.Threading.Tasks;

namespace RescuePC.Software.CQRS.Event;

public interface IEventBus
{
    ValueTask Publish<TEvent>(TEvent @event) where TEvent : IEvent;
}
