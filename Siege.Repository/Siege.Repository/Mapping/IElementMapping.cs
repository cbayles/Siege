using System.Reflection;
using Siege.Repository.Mapping.Conventions.Formatters;

namespace Siege.Repository.Mapping
{
    public interface IElementMapping : IExportable
    {
        object GetValue(object item);
        void Build(DomainMapper mapper, Formatter<PropertyInfo> formatter);
    }
}