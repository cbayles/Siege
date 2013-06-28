using System;
using NHibernate.Bytecode;
using Siege.ServiceLocator;

namespace Siege.Repository.NHibernate
{
    public class SiegeObjectFactory : IObjectsFactory
    {
        private readonly IServiceLocator serviceLocator;

        public SiegeObjectFactory(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public object CreateInstance(Type type)
        {
            return serviceLocator.HasTypeRegistered(type) ? this.serviceLocator.GetInstance(type) : Activator.CreateInstance(type);
        }

        public object CreateInstance(Type type, bool nonPublic)
        {
            return serviceLocator.HasTypeRegistered(type) ? this.serviceLocator.GetInstance(type) : Activator.CreateInstance(type, nonPublic);
        }

        public object CreateInstance(Type type, params object[] ctorArgs)
        {
            return serviceLocator.HasTypeRegistered(type)
                       ? this.serviceLocator.GetInstance(type)
                       : (this.serviceLocator.GetInstance(type) ?? Activator.CreateInstance(type, ctorArgs));
        }
    }
}