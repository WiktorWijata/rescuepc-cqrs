using System;
using System.Threading.Tasks;

namespace RescuePC.Software.CQRS.Query;

public class QueryBus : IQueryBus
{
    private readonly Func<Tuple<Type, Type>, IHandleQuery> _handlersFactory;

    public QueryBus(Func<Tuple<Type, Type>, IHandleQuery> handlersFactory)
    {
        _handlersFactory = handlersFactory;
    }

    public ValueTask<TResult> Execute<TResult>(IQuery<TResult> query)
    {
        var handler = (dynamic)_handlersFactory(new Tuple<Type, Type>(query.GetType(), typeof(TResult)));
        return handler.Handle((dynamic)query);
    }
}
