using System;
using System.Collections.Generic;
using System.Linq;
using Siege.ServiceLocator.InternalStorage;
using Siege.ServiceLocator.Resolution;

namespace Siege.ServiceLocator.WCF
{
    public class WcfResolutionStore : IResolutionStore
    {
        public WcfResolutionStore()
        {
            this.Clear();
        }

        public void Add(List<IResolutionArgument> contextItem)
        {
            WcfOperationContext.Current.Items.Add("WcfResolutionStore_" + Guid.NewGuid(), contextItem);
        }

        public void Clear()
        {
            var list = WcfOperationContext.Current.Items.Keys.OfType<string>().Where(key => key.StartsWith("WcfResolutionStore_")).ToList();

            list.ForEach(k => WcfOperationContext.Current.Items.Remove(k));
        }

        public List<IResolutionArgument> Items
        {
            get
            {
                var items = new List<IResolutionArgument>();

                foreach (var key in WcfOperationContext.Current.Items.Keys.OfType<string>().Where(x => x.StartsWith("WcfResolutionStore_")))
                {
                    items.AddRange((List<IResolutionArgument>)WcfOperationContext.Current.Items[key]);
                }

                return items;
            }
        }

        public void Dispose()
        {
            this.Clear();
        }
    }
}