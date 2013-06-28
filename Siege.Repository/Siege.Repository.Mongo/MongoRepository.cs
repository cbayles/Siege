namespace Siege.Repository.Mongo
{
    public class MongoRepository<TDatabase> : Repository<TDatabase> where TDatabase : IDatabase
    {
        public MongoRepository(MongoUnitOfWorkManager unitOfWork) : base(unitOfWork)
        {
        }
    }
}
