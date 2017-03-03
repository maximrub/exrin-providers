using System;
using Autofac;
using Exrin.Abstraction;
using Exrin.Framework;
using Microsoft.Extensions.DependencyInjection;

namespace Exrin.IOC.AutofacServiceProvider
{
    public abstract class AppInitializer
    {
        private readonly IServiceCollection r_Services;
        private readonly ContainerBuilder r_Container;
        private readonly Action<object> r_SetRoot;  
        private IInjectionProxy m_Resolver;

        protected AppInitializer(PlatformInitializer i_PlatformInitializer, Action<object> i_SetRoot)
        {
            r_SetRoot = i_SetRoot;
            i_PlatformInitializer.Init();
            r_Services = i_PlatformInitializer.Services;
            r_Container = i_PlatformInitializer.Container;
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

        protected ContainerBuilder Container
        {
            get
            {
                return r_Container;
            }
        }

        public void Init()
        {
            ConfigureServices();
            Register();
            InjectionProxy injectionProxy = new InjectionProxy(r_Services, r_Container);
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
        /// Register Components using Autofac container builder
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
