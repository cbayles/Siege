using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Siege.Repository.Mapping.PropertyMappings
{
    public class PropertyMapping<TClass, TType> : PropertyMapping
    {
        public PropertyMapping(Expression<Func<TClass, TType>> expression, string name) : base(((MemberExpression)expression.Body).Member as PropertyInfo, name)
        {
        }

        public PropertyMapping(Expression<Func<TClass, TType>> expression) : base(((MemberExpression)expression.Body).Member as PropertyInfo)
        {
        }
    }

    public class PropertyMapping : ElementMapping, IPropertyMapping
    {
        public PropertyMapping(PropertyInfo property) : this(property, property.Name)
        {
        }

        public PropertyMapping(PropertyInfo property, string name) : base(property)
        {
            this.ColumnName = name;
        }

        public virtual string ColumnName { get; protected set; }

        public override void ExportTo(IDialect exporter)
        {
            exporter.CreateField(this);
        }
    }
}