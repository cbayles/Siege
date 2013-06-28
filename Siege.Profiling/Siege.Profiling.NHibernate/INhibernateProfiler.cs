using NHibernate.Cfg;

namespace Siege.Profiling.NHibernate
{
    public interface INhibernateProfiler
    {
        void WithSql(Configuration cfg);
    }
}
