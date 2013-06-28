using MongoDB.Driver;
using Siege.Repository.UnitOfWork;

namespace Siege.Repository.Mongo
{
    public class MongoUnitOfWorkFactory<TDatabase> : IUnitOfWorkFactory<TDatabase> where TDatabase : IDatabase
    {
        private readonly MongoClient client;
        private readonly MongoServer server;
        private readonly MongoDatabase database;

        public MongoUnitOfWorkFactory(string connectionString)
        {
            this.client = new MongoClient(connectionString);
            this.server = client.GetServer();
            this.database = server.GetDatabase(typeof (TDatabase).Name.ToLower());
        }

        public IUnitOfWork Create()
        {
            return new MongoUnitOfWork(this.database);
        }

        public void Dispose()
        {
        }
    }
}