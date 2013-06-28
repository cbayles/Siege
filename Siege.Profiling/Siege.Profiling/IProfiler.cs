using System;

namespace Siege.Profiling
{
    public interface IProfiler
    {
        void Initialize();
        void Start();
        void Stop();
        IHtmlString Render();
    }

    public class NullProfiler : IProfiler
    {
        public void Initialize()
        {

        }

        public void Start()
        {

        }

        public void Stop()
        {

        }

        public void WithSql(Configuration cfg)
        {

        }

        public IHtmlString Render()
        {
            return null;
        }
    }

    public class DevProfiler : IProfiler
    {
        public void Initialize()
        {
            NHibernateProfiler.Initialize();
        }

        public void Start()
        {
            MiniProfiler.Start();
            MiniProfiler.Settings.SqlFormatter = new SqlServerFormatter();
        }

        public void Stop()
        {
            MiniProfiler.Stop();
        }

        public void WithSql(Configuration cfg)
        {
            cfg.SetProperty("generate_statistics", "true");
            cfg.SetProperty(Environment.ConnectionDriver, typeof(MiniProfilerSql2008ClientDriver).AssemblyQualifiedName);
        }

        public IHtmlString Render()
        {
            return MiniProfiler.RenderIncludes();
        }
    }
}
