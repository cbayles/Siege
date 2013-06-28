using System;
using NHibernate.Bytecode.Lightweight;
using NHibernate.Properties;
using Siege.ServiceLocator;

namespace Siege.Repository.NHibernate
{
    public class SiegeReflectionOptimizer : ReflectionOptimizer
    {
        private readonly IServiceLocator container;

        public SiegeReflectionOptimizer(IServiceLocator container, Type mappedType, IGetter[] getters, ISetter[] setters)
            : base(mappedType, getters, setters)
        {
            this.container = container;
        }

        public override object CreateInstance()
        {
            if (container.HasTypeRegistered(mappedType))
            {
                return container.GetInstance(mappedType);
            }
            else
            {
                return base.CreateInstance();
            }
        }

        protected override void ThrowExceptionForNoDefaultCtor(Type type)
        {
        }
    }
}