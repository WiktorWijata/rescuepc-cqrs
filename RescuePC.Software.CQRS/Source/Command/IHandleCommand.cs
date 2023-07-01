namespace RescuePC.Software.CQRS.Source.Commands;

public interface IHandleCommand { }

public interface IHandleCommand<TCommand> : IHandleCommand where TCommand : ICommand 
{
    ValueTask Handle(TCommand command);
}