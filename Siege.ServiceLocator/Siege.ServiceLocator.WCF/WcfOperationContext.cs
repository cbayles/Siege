using System.Collections;
using System.ServiceModel;

namespace Siege.ServiceLocator.WCF
{
    public class WcfOperationContext : IExtension<OperationContext>
    {
        private readonly IDictionary items;

        private WcfOperationContext()
        {
            this.items = new Hashtable();
        }

        public IDictionary Items
        {
            get { return this.items; }
        }

        public static WcfOperationContext Current
        {
            get
            {
                if (OperationContext.Current == null) return new WcfOperationContext();

                var context = OperationContext.Current.Extensions.Find<WcfOperationContext>();
                if (context == null)
                {
                    context = new WcfOperationContext();
                    OperationContext.Current.Extensions.Add(context);
                }
                return context;
            }
        }

        public void Attach(OperationContext owner) { }
        public void Detach(OperationContext owner) { }


    }
}