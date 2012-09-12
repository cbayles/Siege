using System;
using System.Collections.Generic;
using System.Linq;
using Siege.ServiceLocator.EventHandlers;
using Siege.ServiceLocator.InternalStorage;

namespace Siege.ServiceLocator.WCF
{
    public class WcfContextExecutionStore : IExecutionStore
    {
        private readonly IServiceLocatorStore store;

        protected WcfContextExecutionStore(IServiceLocatorStore store)
        {
            this.store = store;
            this.store.SetStore<IResolutionStore>(new WcfResolutionStore());
            WcfOperationContext.Current.Items["WcfContextExecutionStore_ExecutionCount"] = 0;
        }

        public int Index
        {
            get
            {
                if (!WcfOperationContext.Current.Items.Contains("WcfContextExecutionStore_ExecutionCount")) return 0;

                return (int)WcfOperationContext.Current.Items["WcfContextExecutionStore_ExecutionCount"];
            }
        }

        public static IExecutionStore New(IServiceLocatorStore store)
        {
            return new WcfContextExecutionStore(store);
        }

        public List<Type> RequestedTypes
        {
            get
            {
                var types = new List<Type>();

                foreach (var key in WcfOperationContext.Current.Items.Keys.OfType<string>().Where(x => x.StartsWith("WcfContextExecutionStore_")))
                {
                    types.Add((Type)WcfOperationContext.Current.Items[key]);
                }

                return types;
            }
        }

        public void WireEvent(ITypeResolver typeResolver)
        {
            typeResolver.TypeResolved += OnTypeResolved;
        }

        public void WireEvent(ITypeRequester typeRequestor)
        {
            typeRequestor.TypeRequested += OnTypeRequested;
        }

        public void UnWireEvent(ITypeResolver typeResolver)
        {
            typeResolver.TypeResolved -= OnTypeResolved;
        }

        public void UnWireEvent(ITypeRequester typeRequestor)
        {
            typeRequestor.TypeRequested -= OnTypeRequested;
        }

        void OnTypeResolved(Type type)
        {
            Decrement();
        }

        void OnTypeRequested(Type type)
        {
            AddRequestedType(type);
        }

        public void AddRequestedType(Type type)
        {
            WcfOperationContext.Current.Items.Add("WcfContextExecutionStore_" + Guid.NewGuid(), type);
            Increment();
        }

        private void Increment()
        {
            WcfOperationContext.Current.Items["WcfContextExecutionStore_ExecutionCount"] = this.Index + 1;
        }

        private void Decrement()
        {
            WcfOperationContext.Current.Items["WcfContextExecutionStore_ExecutionCount"] = this.Index - 1;
            if (this.Index == 0)
            {
                this.store.SetStore<IResolutionStore>(new WcfResolutionStore());
            }
        }

        public void Dispose()
        {

        }
    }
}