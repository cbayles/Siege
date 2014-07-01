using Siege.Repository.Finders;

namespace Siege.Repository.MSSQL
{
    public class SQLRepository<TDatabase> where TDatabase : IDatabase
    {
        private readonly SQLUnitOfWorkManager manager;

        public SQLRepository(SQLUnitOfWorkManager manager)
        {
            this.manager = manager;
        }

        public T ExecuteCommand<TCommand, T>() where TCommand : SqlCommand, new()
        {
            var command = new TCommand();

            command.SetUnitOfWork();
            
            return command.Execute<T>();
        }

        public T ExecuteQuery<TQuery, T>() where TQuery : SqlQuery<T>, new()
        {
            var query = new TQuery();

            query.SetUnitOfWork();

            return query.Find();
        }
    }

    public abstract class SqlCommand : Commands.ICommand
    {
        public abstract T Execute<T>();
    }

    public abstract class SqlQuery<T> : IQuery<T>
    {
        public abstract T Find();
    }
}
