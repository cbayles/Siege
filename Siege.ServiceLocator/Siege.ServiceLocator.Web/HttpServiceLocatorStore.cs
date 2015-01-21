/*   Copyright 2009 - 2010 Marcus Bratton

     Licensed under the Apache License, Version 2.0 (the "License");
     you may not use this file except in compliance with the License.
     You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

     Unless required by applicable law or agreed to in writing, software
     distributed under the License is distributed on an "AS IS" BASIS,
     WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
     See the License for the specific language governing permissions and
     limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Siege.ServiceLocator.InternalStorage;
using Siege.ServiceLocator.Registrations.ConditionalAwareness;
using Siege.ServiceLocator.Registrations.InjectionOverrides;

namespace Siege.ServiceLocator.Web
{
    public class HttpServiceLocatorStore : IServiceLocatorStore
    {
        private readonly Dictionary<Type, IStore> stores = new Dictionary<Type, IStore>();

        public HttpServiceLocatorStore() : this(new HttpSessionStore())
        {
        }

        public HttpServiceLocatorStore(IContextStore store)
        {
            AddStore<IContextStore>(store);
            AddStore<IResolutionStore>(new HttpResolutionStore());
            AddStore<IAwarenessStore>(new AwarenessStore());
            AddStore<IInjectionOverrideStore>(new InjectionOverrideStore());
        }


        public void SetStore<TStoreType>(IStore store) where TStoreType : IStore
        {
            this.stores[typeof(TStoreType)] = store;
        }

        public TStoreType Get<TStoreType>() where TStoreType : IStore
        {
            return (TStoreType)this.stores[typeof (TStoreType)];
        }

        public List<TStoreType> All<TStoreType>() where TStoreType : IStore
        {
            return this.stores.Values.ToList().Where(x => x is TStoreType).Cast<TStoreType>().ToList();
        }

        public void ClearScope()
        {
            SetStore<IResolutionStore>(new HttpResolutionStore());
            HttpContext.Current.Items["ResolutionScope"] = null;
        }

        public ResolutionScope GetResolutionScope()
        {
            var firstRun = false;
            var scope = (ResolutionScope)HttpContext.Current.Items["ResolutionScope"];

            if (scope == null)
            {
                firstRun = true;
                scope = new ResolutionScope(this);
                HttpContext.Current.Items["ResolutionScope"] = scope;
            }

            return firstRun ? scope : new NullScope(this);
        }

        public void AddStore<TStoreType>(IStore store) where TStoreType : IStore
        {
            if (!this.stores.ContainsKey(typeof(TStoreType))) this.stores.Add(typeof(TStoreType), store);
        }

        public void Dispose()
        {
            foreach(IStore store in this.stores.Values) store.Dispose();
        }
    }
}