using System;
using LightInject;
using LightInject.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Exrin.IOC
{
    public class LightInjectProvider
    {
        private static LightInjectProvider s_Instance = null;

        private LightInjectProvider(PlatformInitializer i_PlatformInitializer, AppInitializer i_AppInitializer)
        {
            IServiceCollection services = new ServiceCollection();
            ServiceContainer container = new ServiceContainer();

            configureServices(services, i_AppInitializer, i_PlatformInitializer);
            register(container, i_AppInitializer, i_PlatformInitializer);
            populateContainer(container, services);
        }

        /// <exception cref="NullReferenceException" accessor="get">LightInjectProvider Init was not called</exception>
        public static LightInjectProvider Instance
        {
            get
            {
                if(!IsInitialized)
                {
                    throw new NullReferenceException("LightInjectProvider Init was not called");
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

        public ServiceContainer Container { get; private set; }

        public static void Init(PlatformInitializer i_PlatformInitializer, AppInitializer i_AppInitializer)
        {
            if (!IsInitialized)
            {
                s_Instance = new LightInjectProvider(i_PlatformInitializer, i_AppInitializer);
                notifyInitialized(i_PlatformInitializer, i_AppInitializer);
            }
        }

        private static void notifyInitialized(PlatformInitializer i_PlatformInitializer, AppInitializer i_AppInitializer)
        {
            i_PlatformInitializer.OnInitialized(Instance.Container);
            i_AppInitializer.OnInitialized(Instance.Container);
        }

        private void populateContainer(ServiceContainer i_Container, IServiceCollection i_Services)
        {
            i_Container.CreateServiceProvider(i_Services);
            Container = i_Container;
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
