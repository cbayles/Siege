using System;
using System.Collections.Generic;
using System.Linq;
using Siege.ServiceLocator.InternalStorage;
using Siege.ServiceLocator.Registrations.ConditionalAwareness;
using Siege.ServiceLocator.Registrations.InjectionOverrides;

namespace Siege.ServiceLocator.WCF
{
    public class WcfServiceLocatorStore : IServiceLocatorStore
    {
        private readonly Dictionary<Type, IStore> stores = new Dictionary<Type, IStore>();

        public WcfServiceLocatorStore()
            : this(new WcfContextStore())
        {
        }

        public WcfServiceLocatorStore(IContextStore store)
        {
            AddStore<IContextStore>(store);
            AddStore<IResolutionStore>(new WcfResolutionStore());
            AddStore<IExecutionStore>(WcfContextExecutionStore.New(this));
            AddStore<IAwarenessStore>(new AwarenessStore());
            AddStore<IInjectionOverrideStore>(new InjectionOverrideStore());
        }


        public void SetStore<TStoreType>(IStore store) where TStoreType : IStore
        {
            this.stores[typeof(TStoreType)] = store;
        }

        public TStoreType Get<TStoreType>() where TStoreType : IStore
        {
            return (TStoreType)this.stores[typeof(TStoreType)];
        }

        public List<TStoreType> All<TStoreType>() where TStoreType : IStore
        {
            return this.stores.Values.ToList().Where(x => x is TStoreType).Cast<TStoreType>().ToList();
        }

        public void AddStore<TStoreType>(IStore store) where TStoreType : IStore
        {
            if (!this.stores.ContainsKey(typeof(TStoreType))) this.stores.Add(typeof(TStoreType), store);
        }

        public void Dispose()
        {
            foreach (IStore store in this.stores.Values) store.Dispose();
        }
    }
}