using System;
using Exrin.Abstraction;
using LightInject;

namespace Exrin.IOC
{
    public class InjectionProxy : IInjectionProxy
    {
        private readonly ServiceContainer r_Container;

        public InjectionProxy(ServiceContainer i_Container)
        {
            r_Container = i_Container;
        }

        public void Init()
        {
            r_Container.RegisterInstance<IInjectionProxy>(this);
        }

        public void Complete()
        {
        }

        public bool IsRegistered<T>()
        {
            return r_Container.CanGetInstance(typeof(T), string.Empty);
        }

        public void Register<T>(InstanceType i_InstanceType = InstanceType.SingleInstance) where T : class
        {
            switch(i_InstanceType)
            {
                case InstanceType.EachResolve:
                    r_Container.Register<T>();
                    break;
                case InstanceType.SingleInstance:
                    r_Container.Register<T>(new PerContainerLifetime());
                    break;
            }
        }

        public void RegisterInterface<TInterface, TComponent>(InstanceType i_InstanceType = InstanceType.SingleInstance)
            where TInterface : class where TComponent : class, TInterface
        {
            switch(i_InstanceType)
            {
                case InstanceType.EachResolve:
                    r_Container.Register<TInterface, TComponent>();
                    break;
                case InstanceType.SingleInstance:
                    r_Container.Register<TInterface, TComponent>(new PerContainerLifetime());
                    break;
            }
        }

        public void RegisterInstance<TInterface, TComponent>(TComponent i_Instance) where TInterface : class
                                                                                    where TComponent : class, TInterface
        {
            r_Container.RegisterInstance<TInterface>(i_Instance);
        }

        public void RegisterInstance<T>(T i_Instance) where T : class
        {
            r_Container.RegisterInstance<T>(i_Instance);
        }

        public T Get<T>(bool i_Optional = false) where T : class
        {
            return i_Optional ? r_Container.TryGetInstance<T>() : r_Container.GetInstance<T>();
        }

        public object Get(Type i_Type)
        {
            return r_Container.GetInstance(i_Type);
        }
    }
}