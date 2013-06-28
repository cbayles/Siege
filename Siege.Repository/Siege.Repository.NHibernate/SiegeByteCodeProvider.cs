using System;
using NHibernate.Bytecode;
using NHibernate.Properties;
using NHibernate.Type;
using Siege.ServiceLocator;

namespace Siege.Repository.NHibernate
{
    public class SiegeBytecodeProvider : IBytecodeProvider
    {
        private readonly IServiceLocator serviceLocator;

        public SiegeBytecodeProvider(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
            this.ObjectsFactory = new SiegeObjectFactory(serviceLocator);
            this.CollectionTypeFactory = new DefaultCollectionTypeFactory();
        }

        public IReflectionOptimizer GetReflectionOptimizer(Type clazz, IGetter[] getters, ISetter[] setters)
        {
            return new SiegeReflectionOptimizer(serviceLocator, clazz, getters, setters);
        }

        public IProxyFactoryFactory ProxyFactoryFactory
        {
            get { return new SiegeProxyFactory(); }
        }

        public IObjectsFactory ObjectsFactory { get; private set; }
        public ICollectionTypeFactory CollectionTypeFactory { get; private set; }
    }
}
