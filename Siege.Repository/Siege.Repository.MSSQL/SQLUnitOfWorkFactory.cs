using Siege.Repository.UnitOfWork;

namespace Siege.Repository.MSSQL
{
    public class SQLUnitOfWorkFactory<TDatabase> : IUnitOfWorkFactory<TDatabase> where TDatabase : IDatabase
    {
        private readonly string connectionString;

        public SQLUnitOfWorkFactory(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IUnitOfWork Create()
        {
            return new SQLUnitOfWork(connectionString);
        }

        public void Dispose()
        {
            
        }
    }
}