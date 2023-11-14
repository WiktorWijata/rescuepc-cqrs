using Microsoft.Extensions.DependencyInjection;
using RescuePC.Software.CQRS.Event;
using RescuePC.Software.CQRS.Tests.Models;
using Xunit;

namespace RescuePC.Software.CQRS.Tests
{
    public class CQRSDependencyInjectionTests
    {
        private readonly IServiceProvider _serviceProvider;

        public CQRSDependencyInjectionTests()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddCQRS(typeof(TestEvent).Assembly);
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        [Fact]
        public async Task EventBus_ShouldBeResolved_AndHandleEvents()
        {
            var eventBus = _serviceProvider.GetService<IEventBus>();
            Assert.NotNull(eventBus);
            var @event = new TestEvent();
            await eventBus.Publish(@event);
            Assert.Equal(2, @event.TimesInvoked);
        }
    }
}