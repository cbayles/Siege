using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Siege.Repository.Mapping.Conventions;
using Siege.Repository.Mapping.Conventions.Formatters;

namespace Siege.Repository.Mapping
{
    public class DomainMapper
    {
        private readonly Formatter<PropertyInfo> formatter;
        private readonly Mappings mappings = new Mappings();
        private readonly List<IConvention> conventions = new List<IConvention>();

        public Mappings Mappings
        {
            get { return mappings; }
        }

        public DomainMapper(Formatter<PropertyInfo> formatter)
        {
            this.formatter = formatter;
        }

        public void Add<TClass>(Action<DomainMapping<TClass>> mapping) where TClass : class
        {
            var map = new DomainMapping<TClass>();
            mapping(map);
            this.Mappings.Add<TClass>(map);
        }

        public void Add(Type type, Action<DomainMapping> mapping)
        {
            var map = new DomainMapping(type);
            mapping(map);
            this.Mappings.Add(type, map);
        }

        public void Override(Type type, Action<DomainMapping> mapping)
        {
            var map = new DomainMapping(type);
            mapping(map);
            this.Mappings.Add(type, map);
        }

        public void Override<TClass>(Action<DomainMapping<TClass>> mapping) where TClass : class
        {
            var map = new DomainMapping<TClass>();
            mapping(map);
            this.Mappings.Add<TClass>(map);
        }

        public IDomainMapping For(Type type)
        {
            if(!this.mappings.Contains(type)) Add(type);

            return this.mappings.For(type);
        }

        public void Add<TClass>() where TClass : class
        {
            this.Mappings.Add<TClass>(new DomainMapping<TClass>());
        }

        public void Add<TDomainMapping, TClass>(TDomainMapping mapping) where TClass : class where TDomainMapping : DomainMapping<TClass>, new()
        {
            this.Mappings.Add<TClass>(mapping);
        }

        public void Add(DomainMapping mapping, Type type)
        {
            this.Mappings.Add(type, mapping);
        }

        public void AddFromAssemblyOf<TDomainMapping>()
        {
            var assembly = typeof (TDomainMapping).Assembly;

            foreach (var type in assembly.GetExportedTypes().Where(x => typeof(DomainMapping).IsAssignableFrom(x) && !x.IsAbstract).Select(types => types))
            {
                var mapping = (DomainMapping)type.GetConstructor(new Type[] {}).Invoke(new object[] {});
                Add(mapping, mapping.Type);
            }
        }

        public void Add(Type type)
        {
            this.mappings.Add(type, new DomainMapping(type));
        }

        public void UseConvention(Action<ClassConvention> convention)
        {
            var instance = new ClassConvention(this);

            convention(instance);

            this.conventions.Add(instance);
        }

        public void UseConvention<TConvention>() where TConvention : class, IConvention, new()
        {
            this.conventions.Add(new TConvention());
        }

        public void Build()
        {
            foreach(IConvention convention in this.conventions)
            {
                foreach(Type type in mappings.MappedTypes)
                {
                    this.For(type).Map(mapping => convention.Map(type, mapping));
                }
            }
         
            foreach (Type type in mappings.MappedTypes)
            {
                this.For(type).SubMappings.ForEach(x => x.Build(this, formatter));
            }
        }
    }
}