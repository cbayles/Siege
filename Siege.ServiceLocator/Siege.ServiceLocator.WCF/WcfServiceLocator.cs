namespace Siege.ServiceLocator.WCF
{
    public class WcfServiceLocator : ServiceLocator
    {
        public WcfServiceLocator(IServiceLocatorAdapter serviceLocator)
            : base(serviceLocator, new WcfServiceLocatorStore())
        {
        }
    }
}
