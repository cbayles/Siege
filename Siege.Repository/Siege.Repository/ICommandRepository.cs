using Siege.Repository.Commands;

namespace Siege.Repository
{
    public interface ICommandRepository<TPersistenceModel> where TPersistenceModel : IDatabase
    {
        T ExecuteCommand<TCommand, T>() where TCommand : ICommand, new();
    }
}