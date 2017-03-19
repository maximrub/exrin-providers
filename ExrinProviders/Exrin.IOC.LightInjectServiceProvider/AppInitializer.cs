using System;
using Exrin.Abstraction;
using LightInject;
using Microsoft.Extensions.DependencyInjection;
using Xamarin.Forms;

namespace Exrin.IOC.LightInjectServiceProvider
{
    public abstract class AppInitializer
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

        /// <summary>
        /// Register Exrin Framework Assemblies
        /// </summary>
        /// <param name="i_Bootstrapper"></param>
        protected internal virtual void RegisterFrameworkAssemblies(Exrin.Framework.Bootstrapper i_Bootstrapper)
        {
        }

        protected internal virtual void OnInitialized(ServiceContainer i_Container)
        {
            if (Initialized != null)
            {
                Initialized.Invoke(this, i_Container);
            }
        }

        protected internal virtual Action<object> GetRoot()
        {
            return (i_View) => { Application.Current.MainPage = i_View as Page; };
        }
    }
}
