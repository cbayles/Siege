using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Siege.Repository.Mapping.PropertyMappings
{
    public class ComponentMapping<TClass, TType> : ComponentMapping
    {
        public ComponentMapping(Expression<Func<TClass, TType>> expression, string name) : base(((MemberExpression)expression.Body).Member as PropertyInfo, name)
        {
        }

        public ComponentMapping(Expression<Func<TClass, TType>> expression)
            : base(((MemberExpression)expression.Body).Member as PropertyInfo)
        {
        }

        public ComponentMapping<TClass, TType> MapProperty<TComponentType>(Expression<Func<TType, TComponentType>> expression)
        {
            this.subMappings.Add(new PropertyMapping<TType, TComponentType>(expression));
            return this;
        }

        public ComponentMapping<TClass, TType> MapProperty<TComponentType>(Expression<Func<TType, TComponentType>> expression, string columnName)
        {
            this.subMappings.Add(new PropertyMapping<TType, TComponentType>(expression, columnName));
            return this;
        }

        public void ExportTo(IDialect exporter)
        {
            foreach (var mapping in subMappings)
            {
                mapping.ExportTo(exporter);
            }
        }
    }

    public class ComponentMapping : ElementMapping, IExportable
    {
        private readonly string name;
        public string Name { get { return name; } }

        public ComponentMapping(PropertyInfo property) : base(property)
        {
            this.name = Property.Name;
        }

        public ComponentMapping(PropertyInfo property, string name) : base(property)
        {
            this.name = name;
        }

        protected readonly List<IElementMapping> subMappings = new List<IElementMapping>();
        public List<IElementMapping> SubMappings { get { return subMappings; } }

        public ComponentMapping MapProperty(PropertyInfo property, string name)
        {
            this.subMappings.Add(new PropertyMapping(property, name));
            return this;
        }

        public void ExportTo(IDialect exporter)
        {
            foreach (var mapping in subMappings)
            {
                mapping.ExportTo(exporter);
            }
        }
    }
}