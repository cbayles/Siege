using System;
using System.Collections.Generic;
using System.Linq;
using Siege.ServiceLocator.InternalStorage;

namespace Siege.ServiceLocator.WCF
{
    public class WcfContextStore : IContextStore
    {
        public void Add(object contextItem)
        {
            WcfOperationContext.Current.Items.Add("WcfContextStore_" + contextItem.GetType() + Guid.NewGuid().ToString(), contextItem);
        }

        public List<object> Items
        {
            get
            {
                var items = new List<object>();

                foreach (string item in WcfOperationContext.Current.Items)
                {
                    items.Add(WcfOperationContext.Current.Items[item]);
                }

                return items;
            }
        }

        public void Clear()
        {
            var items = WcfOperationContext.Current.Items.Keys.OfType<string>().Where(x => x.StartsWith("WcfContextStore_")).ToList();

            foreach (var item in items)
            {
                WcfOperationContext.Current.Items.Remove(item);
            }
        }

        public void Dispose()
        {

        }
    }
}