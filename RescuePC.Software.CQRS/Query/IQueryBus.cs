using System.Threading.Tasks;

namespace RescuePC.Software.CQRS.Query;

public interface IQueryBus
{
    ValueTask<TResult> Execute<TResult>(IQuery<TResult> query);
}
