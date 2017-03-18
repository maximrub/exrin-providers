using System;
using Exrin.Abstraction;
using LightInject;
using Microsoft.Extensions.DependencyInjection;

namespace Exrin.IOC.LightInjectServiceProvider
{
    public class Bootstrapper
    {
        private static Bootstrapper s_Instance;

        private Bootstrapper(PlatformInitializer i_PlatformInitializer, AppInitializer i_AppInitializer, Action<object> i_SetRoot)
        {
            IServiceCollection services = new ServiceCollection();
            ServiceContainer container = new ServiceContainer();

            configureServices(services, i_AppInitializer, i_PlatformInitializer);
            register(container, i_AppInitializer, i_PlatformInitializer);
            configureExrin(container, services, i_AppInitializer, i_SetRoot);
            notifyInitialized(i_PlatformInitializer, i_AppInitializer);
        }

        /// <exception cref="NullReferenceException" accessor="get">Bootstrapper Init was not called</exception>
        public static Bootstrapper Instance
        {
            get
            {
                if(s_Instance == null)
                {
                    throw new NullReferenceException("Bootstrapper Init was not called");
                }

                return s_Instance;
            }
        }

        public IInjectionProxy Resolver { get; private set; }

        public static void Init(PlatformInitializer i_PlatformInitializer, AppInitializer i_AppInitializer, Action<object> i_SetRoot)
        {
            if (s_Instance == null)
            {
                s_Instance = new Bootstrapper(i_PlatformInitializer, i_AppInitializer, i_SetRoot);
            }
        }

        private void configureExrin(
            ServiceContainer i_Container,
            IServiceCollection i_Services,
            AppInitializer i_AppInitializer,
            Action<object> i_SetRoot)
        {
            InjectionProxy injectionProxy = new InjectionProxy(i_Container, i_Services);
            Exrin.Framework.Bootstrapper bootstrapper = new Exrin.Framework.Bootstrapper(injectionProxy, i_SetRoot);
            i_AppInitializer.RegisterFrameworkAssemblies(bootstrapper);
            Resolver = bootstrapper.Init();
        }

        private void notifyInitialized(PlatformInitializer i_PlatformInitializer, AppInitializer i_AppInitializer)
        {
            i_PlatformInitializer.OnInitialized(Resolver);
            i_AppInitializer.OnInitialized(Resolver);
        }

        private void register(ServiceContainer i_Container, AppInitializer i_AppInitializer, PlatformInitializer i_PlatformInitializer)
        {
            i_PlatformInitializer.Register(i_Container);
            i_AppInitializer.Register(i_Container);
        }

        private void configureServices(IServiceCollection i_Services, AppInitializer i_AppInitializer, PlatformInitializer i_PlatformInitializer)
        {
            i_PlatformInitializer.ConfigureServices(i_Services);
            i_AppInitializer.ConfigureServices(i_Services);
        }
    }
}
