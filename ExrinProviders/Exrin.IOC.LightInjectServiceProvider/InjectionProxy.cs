using System;
using System.Linq;
using Exrin.Abstraction;
using LightInject;
using Microsoft.Extensions.DependencyInjection;
using LightInject.Microsoft.DependencyInjection;

namespace Exrin.IOC.LightInjectServiceProvider
{
    public class InjectionProxy : IInjectionProxy
    {
        private readonly ServiceContainer r_Container;
        private readonly IServiceCollection r_Services;

        public InjectionProxy(ServiceContainer i_Container, IServiceCollection i_Services)
        {
            r_Container = i_Container;
            r_Services = i_Services;
        }

        public void Init()
        {
            r_Container.CreateServiceProvider(r_Services);
            r_Container.RegisterInstance<IInjectionProxy>(this);
        }

        public void Complete()
        {
        }

        public bool IsRegistered<T>()
        {
            return r_Container.AvailableServices.Any(i_Service => i_Service.ServiceType.Equals(typeof(T)));
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