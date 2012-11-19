using System.Linq;
using Siege.Repository.UnitOfWork;
using Siege.ServiceLocator.WCF;

namespace Siege.Repository.WCF
{
    public class WcfUnitOfWorkStore : IUnitOfWorkStore
    {
        public void Dispose()
        {
            if (SessionExists())
            {
                var unitsOfWork =
                    WcfOperationContext.Current.Items.Keys.OfType<string>().Where(
                        x => x.StartsWith("WcfUnitOfWorkStore.CurrentUnitOfWork_")).ToList();

                foreach (var key in unitsOfWork)
                {
                    var currentUnitOfWork = WcfOperationContext.Current.Items[key] as IUnitOfWork;

                    if (currentUnitOfWork == null) continue;
                    currentUnitOfWork.Dispose();

                    WcfOperationContext.Current.Items.Remove(key);
                }
            }
        }

        public IUnitOfWork CurrentFor<TDatabase>() where TDatabase : IDatabase
        {
            if (SessionExists())
            {
                return
                    WcfOperationContext.Current.Items["WcfUnitOfWorkStore.CurrentUnitOfWork_" + typeof(TDatabase)]
                    as IUnitOfWork;
            }
            return null;
        }

        public void SetUnitOfWork<TDatabase>(IUnitOfWork unitOfWork)
            where TDatabase : IDatabase
        {
            if (SessionExists())
            {
                WcfOperationContext.Current.Items["WcfUnitOfWorkStore.CurrentUnitOfWork_" + typeof(TDatabase)] =
                    unitOfWork;
            }
        }

        private static bool SessionExists()
        {
            return WcfOperationContext.Current.Items != null;
        }
    }
}
