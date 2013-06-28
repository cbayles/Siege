using System;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Siege.Repository.UnitOfWork;

namespace Siege.Repository.Mongo
{
    public interface IDocumentEntity
    {
        ObjectId ObjectId { get; set; }
    }

    public class MongoUnitOfWork : IUnitOfWork
    {
        private readonly MongoDatabase database;

        public MongoUnitOfWork(MongoDatabase database)
        {
            this.database = database;
        }

        public void Dispose()
        {
        }

        public T Get<T>(object id) where T : class
        {
            return this.database.GetCollection<T>(typeof (T).Name.ToLower()).FindOneById(new ObjectId(id.ToString()));
        }

        public void Transact(Action action)
        {
            action();
        }

        public T Transact<T>(Func<T> action) where T : class
        {
            return action();
        }

        public void Save<T>(T item) where T : class
        {
            if (!(item is IDocumentEntity)) throw new Exception("Type must implement IDocumentEntity interface.");
            
            var result = this.database.GetCollection<T>(typeof (T).Name.ToLower()).Save(item, WriteConcern.Acknowledged);

            if(!result.Ok) throw new Exception("Unable to process the transaction.");
        }

        public void Delete<T>(T item) where T : class
        {
            if (!(item is IDocumentEntity)) throw new Exception("Type must implement IDocumentEntity interface.");
            
            var query = MongoDB.Driver.Builders.Query.EQ("__id", (item as IDocumentEntity).ObjectId);
            var result = this.database.GetCollection<T>(typeof(T).Name.ToLower()).Remove(query, WriteConcern.Acknowledged);

            if (!result.Ok) throw new Exception("Unable to process the transaction.");
        }

        public IQueryable<T> Query<T>() where T : class
        {
            return this.database.GetCollection<T>(typeof (T).Name.ToLower()).AsQueryable();
        }
    }
}