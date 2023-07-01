using System.Threading.Tasks;

namespace RescuePC.Software.CQRS.Source.Query;

public interface IHandleQuery { }

public interface IHandleQuery<in TQuery, TResult> : IHandleQuery where TQuery : IQuery<TResult>
{
    ValueTask<TResult> Handle(TQuery query);
}

