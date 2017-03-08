using LightInject;
using Microsoft.Extensions.DependencyInjection;

namespace Exrin.IOC.LightInjectServiceProvider
{
    public abstract class PlatformInitializer
    {
        protected PlatformInitializer()
        {
            Container = new ServiceContainer();
            Services = new ServiceCollection();
        }

        public IServiceCollection Services { get; private set; }

        public ServiceContainer Container { get; private set; }

        internal void Init()
        {
            ConfigureServices();
            Register();
        }

        /// <summary>
        /// Configure Framework services
        /// </summary>
        protected virtual void ConfigureServices()
        {
        }

        /// <summary>
        /// Register Components using LightInject container
        /// </summary>
        protected virtual void Register()
        {
        }
    }
}