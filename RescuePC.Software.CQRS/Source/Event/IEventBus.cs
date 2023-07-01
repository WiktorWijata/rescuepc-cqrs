using System.Threading.Tasks;

namespace RescuePC.Software.CQRS.Source.Event;

public interface IEventBus
{
    ValueTask Publish<TEvent>(TEvent @event) where TEvent : IEvent;
}
