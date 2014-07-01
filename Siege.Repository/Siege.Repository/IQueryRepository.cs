using Siege.Repository.Finders;

namespace Siege.Repository
{
    public interface IQueryRepository<TPersistenceModel> where TPersistenceModel : IDatabase
    {
        T ExecuteQuery<TQuery, T>() where TQuery : IQuery<T>, new();
    }
}