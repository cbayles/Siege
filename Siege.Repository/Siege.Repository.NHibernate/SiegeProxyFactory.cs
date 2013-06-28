using System;
using NHibernate.Bytecode;
using NHibernate.Proxy;

namespace Siege.Repository.NHibernate
{
    public class SiegeProxyFactory : IProxyFactoryFactory
    {
        public SiegeProxyFactory()
        {
            this.ProxyValidator = new DynProxyTypeValidator();
            this.ProxyFactory = new DefaultProxyFactory();
        }

        public IProxyFactory BuildProxyFactory()
        {
            return this.ProxyFactory;
        }

        public bool IsInstrumented(Type entityClass)
        {
            return true;
        }

        public bool IsProxy(object entity)
        {
            return entity is INHibernateProxy;
        }

        public IProxyValidator ProxyValidator { get; private set; }
        public IProxyFactory ProxyFactory { get; private set; }
    }
}