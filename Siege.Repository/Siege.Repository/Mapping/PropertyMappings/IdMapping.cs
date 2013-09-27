using System;
using System.Linq.Expressions;
using System.Reflection;
using Siege.Repository.Mapping.Conventions.Formatters;

namespace Siege.Repository.Mapping.PropertyMappings
{
    public class IdMapping<TClass, TType> : IdMapping
    {
        public IdMapping(Expression<Func<TClass, TType>> expression, string name) : base(((MemberExpression)expression.Body).Member as PropertyInfo, name)
        {
        }

        public IdMapping(Expression<Func<TClass, TType>> expression, Formatter<Type> formatter) : base(((MemberExpression)expression.Body).Member as PropertyInfo, typeof(TClass), formatter)
        {
        }
    }

    public class IdMapping : PropertyMapping
    {
        public IdMapping(PropertyInfo property, Type type, Formatter<Type> keyFormatter) : base(property)
        {
            this.ColumnName = keyFormatter.Format(type);
        }

        public IdMapping(PropertyInfo property, string name) : base(property)
        {
            this.ColumnName = name;
        }
    }
}