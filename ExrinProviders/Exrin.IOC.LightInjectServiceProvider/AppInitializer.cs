using System;
using Exrin.Abstraction;
using Exrin.Framework;
using LightInject;
using Microsoft.Extensions.DependencyInjection;

namespace Exrin.IOC.LightInjectServiceProvider
{
    public abstract class AppInitializer
    {
        private readonly IServiceCollection r_Services;
        private readonly ServiceContainer r_Container;
        private readonly PlatformInitializer r_PlatformInitializer;
        private readonly Action<object> r_SetRoot;  
        private IInjectionProxy m_Resolver;

        protected AppInitializer(PlatformInitializer i_PlatformInitializer, Action<object> i_SetRoot)
        {
            r_PlatformInitializer = i_PlatformInitializer;
            r_SetRoot = i_SetRoot;
            r_Services = r_PlatformInitializer.Services;
            r_Container = r_PlatformInitializer.Container;
        }

        public IInjectionProxy Resolver
        {
            get
            {
                return m_Resolver;
            }
        }

        protected IServiceCollection Services
        {
            get
            {
                return r_Services;
            }
        }

        protected ServiceContainer Container
        {
            get
            {
                return r_Container;
            }
        }

        public void Init()
        {
            r_PlatformInitializer.Init();
            ConfigureServices();
            Register();
            InjectionProxy injectionProxy = new InjectionProxy(r_Container, r_Services);
            Bootstrapper bootstrapper = new Bootstrapper(injectionProxy, r_SetRoot);
            RegisterFrameworkAssemblies(bootstrapper);
            m_Resolver = bootstrapper.Init();
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

        /// <summary>
        /// Register Exrin Framework Assemblies
        /// </summary>
        /// <param name="i_Bootstrapper"></param>
        protected virtual void RegisterFrameworkAssemblies(Bootstrapper i_Bootstrapper)
        {
        }
    }
}
