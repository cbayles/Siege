using System;
using System.ServiceModel.Activation;

namespace Siege.ServiceLocator.WCF
{
    public class WcfServiceLocatorServiceHostFactory : ServiceHostFactory
    {
        protected override System.ServiceModel.ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            return new WcfServiceLocatorServiceHost(serviceType, baseAddresses);
        }
    }
}