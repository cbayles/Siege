using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Siege.ServiceLocator.WCF
{
    public class WcfServiceLocatorBehavior : IServiceBehavior
    {
        private readonly IServiceLocator serviceLocator;

        public WcfServiceLocatorBehavior(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher channelDispatcher in serviceHostBase.ChannelDispatchers)
            {
                foreach (EndpointDispatcher endpointDispatcher in channelDispatcher.Endpoints)
                {
                    if (endpointDispatcher.ContractName != "IMetadataExchange")
                    {
                        string contractName = endpointDispatcher.ContractName;
                        ServiceEndpoint serviceEndpoint = serviceDescription.Endpoints.FirstOrDefault(e => e.Contract.Name == contractName);
                        if (serviceEndpoint != null)
                        {
                            endpointDispatcher.DispatchRuntime.InstanceProvider = new WcfServiceLocatorInstanceProvider(this.serviceLocator, serviceEndpoint.Contract.ContractType);
                        }
                    }
                }
            }
        }
    }
}
