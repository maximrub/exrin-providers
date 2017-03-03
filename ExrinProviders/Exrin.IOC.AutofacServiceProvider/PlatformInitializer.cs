using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace Exrin.IOC.AutofacServiceProvider
{
    public abstract class PlatformInitializer
    {
        protected PlatformInitializer()
        {
            Container = new ContainerBuilder();
            Services = new ServiceCollection();
        }

        public IServiceCollection Services { get; private set; }

        public ContainerBuilder Container { get; private set; }

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
        /// Register Components using Autofac container builder
        /// </summary>
        protected virtual void Register()
        {
        }
    }
}