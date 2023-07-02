using System.Threading.Tasks;

namespace RescuePC.Software.CQRS.Query;

public interface IHandleQuery { }

public interface IHandleQuery<in TQuery, TResult> : IHandleQuery where TQuery : IQuery<TResult>
{
    ValueTask<TResult> Handle(TQuery query);
}

