using System.Threading.Tasks;

namespace RescuePC.Software.CQRS.Source.Query;

public interface IQueryBus
{
    ValueTask<TResult> Execute<TResult>(IQuery<TResult> query);
}
