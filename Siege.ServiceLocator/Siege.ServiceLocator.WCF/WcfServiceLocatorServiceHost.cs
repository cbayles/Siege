using System;
using System.ServiceModel;
using System.Web;

namespace Siege.ServiceLocator.WCF
{
    public class WcfServiceLocatorServiceHost : ServiceHost
    {
        public WcfServiceLocatorServiceHost(Type serviceType, params Uri[] baseAddresses) : base(serviceType, baseAddresses)
        {
        }

        protected override void OnOpening()
        {
            base.OnOpening();
            if (HttpContext.Current.ApplicationInstance != null && HttpContext.Current.ApplicationInstance is IServiceLocatorAccessor)
            {
                var application = (IServiceLocatorAccessor)HttpContext.Current.ApplicationInstance;
                if (this.Description.Behaviors.Find<WcfServiceLocatorBehavior>() == null)
                {
                    this.Description.Behaviors.Add(new WcfServiceLocatorBehavior(application.ServiceLocator));
                }
            }
        }
    }
}
