using System;
using LightInject;
using Microsoft.Extensions.DependencyInjection;

namespace Exrin.IOC
{
    public abstract class PlatformInitializer
    {
        public event EventHandler<ServiceContainer> Initialized;

        /// <summary>
        /// Configure Framework services
        /// </summary>
        protected internal virtual void ConfigureServices(IServiceCollection i_Services)
        {
        }

        /// <summary>
        /// Register Components using LightInject container
        /// </summary>
        protected internal virtual void Register(ServiceContainer i_Container)
        {
        }

        protected internal virtual void OnInitialized(ServiceContainer i_Container)
        {
            if(Initialized != null)
            {
                Initialized.Invoke(this, i_Container);
            }
        }
    }
}
