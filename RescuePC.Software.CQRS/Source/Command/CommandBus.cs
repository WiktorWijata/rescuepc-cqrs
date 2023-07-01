using System;
using System.Threading.Tasks;

namespace RescuePC.Software.CQRS.Source.Commands;

public class CommandBus : ICommandBus
{
    private readonly Func<Type, IHandleCommand> _handlersFactory;

    public CommandBus(Func<Type, IHandleCommand> handlersFactory)
    {
        _handlersFactory = handlersFactory;
    }

    public ValueTask Send<TCommand>(TCommand command) where TCommand : ICommand
    {
        var handler = (IHandleCommand<TCommand>)_handlersFactory(typeof(TCommand));
        return handler.Handle(command);
    }
}
