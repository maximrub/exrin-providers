using System;
using Exrin.Abstraction;
using LightInject;
using Microsoft.Extensions.DependencyInjection;

namespace Exrin.IOC.LightInjectServiceProvider
{
    public class Bootstrapper
    {
        private static Bootstrapper s_Instance = null;
        private Exrin.Framework.Bootstrapper m_Bootstrapper;

        private Bootstrapper(PlatformInitializer i_PlatformInitializer, AppInitializer i_AppInitializer)
        {
            IServiceCollection services = new ServiceCollection();
            ServiceContainer container = new ServiceContainer();

            configureServices(services, i_AppInitializer, i_PlatformInitializer);
            register(container, i_AppInitializer, i_PlatformInitializer);
            configureExrin(container, services, i_AppInitializer);
            configureContainer(container);
        }

        /// <exception cref="NullReferenceException" accessor="get">Bootstrapper Init was not called</exception>
        public static Bootstrapper Instance
        {
            get
            {
                if(!IsInitialized)
                {
                    throw new NullReferenceException("Bootstrapper Init was not called");
                }

                return s_Instance;
            }
        }

        public static bool IsInitialized
        {
            get
            {
                return s_Instance != null;
            }
        }

        public IInjectionProxy InjectionProxy { get; private set; }

        public ServiceContainer Container { get; private set; }

        public static void Init(PlatformInitializer i_PlatformInitializer, AppInitializer i_AppInitializer)
        {
            if (!IsInitialized)
            {
                s_Instance = new Bootstrapper(i_PlatformInitializer, i_AppInitializer);
                notifyInitialized(i_PlatformInitializer, i_AppInitializer);
            }
        }

        private static void notifyInitialized(PlatformInitializer i_PlatformInitializer, AppInitializer i_AppInitializer)
        {
            i_PlatformInitializer.OnInitialized(Instance.Container);
            i_AppInitializer.OnInitialized(Instance.Container);
        }

        private void configureContainer(ServiceContainer i_Container)
        {
            Container = i_Container;
        }

        private void configureExrin(
            ServiceContainer i_Container,
            IServiceCollection i_Services,
            AppInitializer i_AppInitializer)
        {
            InjectionProxy injectionProxy = new InjectionProxy(i_Container, i_Services);
            m_Bootstrapper = new Exrin.Framework.Bootstrapper(injectionProxy, i_AppInitializer.GetRoot());
            i_AppInitializer.RegisterFrameworkAssemblies(m_Bootstrapper);
            InjectionProxy = m_Bootstrapper.Init();
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
