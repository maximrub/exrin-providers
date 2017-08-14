using System;
using LightInject;

namespace Exrin.IOC
{
    public class LightInjectProvider
    {
        private static LightInjectProvider s_Instance = null;

        private LightInjectProvider(PlatformInitializer i_PlatformInitializer, AppInitializer i_AppInitializer)
        {
            Container = new ServiceContainer();

            register(i_AppInitializer, i_PlatformInitializer);
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

        private void register(AppInitializer i_AppInitializer, PlatformInitializer i_PlatformInitializer)
        {
            i_PlatformInitializer.Register(Container);
            i_AppInitializer.Register(Container);
        }
    }
}
