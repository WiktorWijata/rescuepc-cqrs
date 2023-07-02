using System.Threading.Tasks;

namespace RescuePC.Software.CQRS.Command;

public interface ICommandBus
{
    ValueTask Send<TCommand>(TCommand command) where TCommand : ICommand;
}
