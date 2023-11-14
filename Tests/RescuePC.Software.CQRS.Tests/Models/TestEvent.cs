using RescuePC.Software.CQRS.Event;

namespace RescuePC.Software.CQRS.Tests.Models;

public class TestEvent : IEvent
{
    public int TimesInvoked { get; set; }
}
