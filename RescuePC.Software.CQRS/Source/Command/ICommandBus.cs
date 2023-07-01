using System.Threading.Tasks;

namespace RescuePC.Software.CQRS.Source.Commands;

public interface ICommandBus
{
    ValueTask Send<TCommand>(TCommand command) where TCommand : ICommand;
}
