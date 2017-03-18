using System;
using Exrin.Abstraction;
using LightInject;
using Microsoft.Extensions.DependencyInjection;

namespace Exrin.IOC.LightInjectServiceProvider
{
    public abstract class PlatformInitializer
    {
        public event EventHandler<IInjectionProxy> Initialized;

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

        protected internal virtual void OnInitialized(IInjectionProxy i_Resolver)
        {
            if(Initialized != null)
            {
                Initialized.Invoke(this, i_Resolver);
            }
        }
    }
}
