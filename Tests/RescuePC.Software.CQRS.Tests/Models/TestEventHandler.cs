using RescuePC.Software.CQRS.Event;

namespace RescuePC.Software.CQRS.Tests.Models;

public class TestEventHandler : IHandleEvent<TestEvent>
{
    public ValueTask Handle(TestEvent @event)
    {
        @event.TimesInvoked++;
        return ValueTask.CompletedTask;
    }
}
