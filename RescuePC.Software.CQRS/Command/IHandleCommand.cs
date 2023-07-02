using System.Threading.Tasks;

namespace RescuePC.Software.CQRS.Command;

public interface IHandleCommand { }

public interface IHandleCommand<TCommand> : IHandleCommand where TCommand : ICommand
{
    ValueTask Handle(TCommand command);
}