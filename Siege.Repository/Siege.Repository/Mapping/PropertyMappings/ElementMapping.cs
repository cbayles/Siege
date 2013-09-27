using System.Reflection;
using Siege.Repository.Mapping.Conventions.Formatters;

namespace Siege.Repository.Mapping.PropertyMappings
{
    public abstract class ElementMapping : IElementMapping
    {
        protected ElementMapping(PropertyInfo property)
        {
            this.Property = property;
        }

        public PropertyInfo Property { get; protected set; }
        
        public virtual object GetValue(object item)
        {
            return this.Property.GetValue(item, new object[0]);
        }

        public virtual void ExportTo(IDialect exporter)
        {
            
        }

        public virtual void Build(DomainMapper mapper, Formatter<PropertyInfo> formatter) { }
    }
}