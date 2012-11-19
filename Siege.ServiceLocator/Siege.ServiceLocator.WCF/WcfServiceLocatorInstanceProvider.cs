using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace Siege.ServiceLocator.WCF
{
    public class WcfServiceLocatorInstanceProvider : IInstanceProvider
    {
        private readonly IServiceLocator serviceLocator;
        private readonly Type contractType;

        public WcfServiceLocatorInstanceProvider(IServiceLocator serviceLocator, Type contractType)
        {
            this.serviceLocator = serviceLocator;
            this.contractType = contractType;
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            return GetInstance(instanceContext, null);
        }

        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            return this.serviceLocator.GetInstance(this.contractType);
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
        }
    }
}
