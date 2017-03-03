using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Builder;
using Autofac.Extensions.DependencyInjection;
using Exrin.Abstraction;
using Microsoft.Extensions.DependencyInjection;

namespace Exrin.IOC.AutofacServiceProvider
{
    public class InjectionProxy : IInjectionProxy
    {
        private readonly IServiceCollection r_Services;
        private readonly ContainerBuilder r_Builder;
        private readonly LinkedList<Type> r_RegisteredTypes;
        private IServiceProvider m_ServiceProvider;
        private IContainer m_Container;

        /// <summary>
        /// C'tor for InjectionProxy
        /// </summary>
        /// <param name="i_Services">framework services collection to register</param>
        /// <param name="i_Builder">Autofac container builder</param>
        public InjectionProxy(IServiceCollection i_Services, ContainerBuilder i_Builder)
        {
            r_Services = i_Services;
            r_Builder = i_Builder;
            r_RegisteredTypes = new LinkedList<Type>();
        }

        public void Init()
        {
            r_Builder.Populate(r_Services);
            r_Builder.RegisterInstance<IInjectionProxy>(this).SingleInstance();
        }

        public void Complete()
        {
            m_Container = r_Builder.Build();
            m_ServiceProvider = new Autofac.Extensions.DependencyInjection.AutofacServiceProvider(m_Container);
            r_RegisteredTypes.Clear();
        }

        public bool IsRegistered<T>()
        {
            return m_Container != null ? m_Container.IsRegistered<T>() : r_RegisteredTypes.Contains(typeof(T));
        }

        public void Register<T>(InstanceType i_InstanceType = InstanceType.SingleInstance) where T : class
        {
            register(r_Builder.RegisterType<T>(), i_InstanceType);
            r_RegisteredTypes.AddLast(typeof(T));
        }

        public void RegisterInterface<TInterface, TComponent>(InstanceType i_InstanceType = InstanceType.SingleInstance) 
            where TInterface : class 
            where TComponent : class, TInterface
        {
            register(r_Builder.RegisterType<TComponent>().As<TInterface>(), i_InstanceType);
            r_RegisteredTypes.AddLast(typeof(TInterface));
        }

        public void RegisterInstance<TInterface, TComponent>(TComponent i_Instance) 
            where TInterface : class 
            where TComponent : class, TInterface
        {
            r_Builder.RegisterInstance<TComponent>(i_Instance).As<TInterface>().SingleInstance();
            r_RegisteredTypes.AddLast(typeof(TInterface));
        }

        public void RegisterInstance<T>(T i_Instance) 
            where T : class
        {
            r_Builder.RegisterInstance(i_Instance).As<T>().SingleInstance();
            r_RegisteredTypes.AddLast(typeof(T));
        }

        /// <exception cref="NullReferenceException">IInjectionProxy not initialized.</exception>
        public object Get(Type i_Type)
        {
            if(m_ServiceProvider == null)
            {
                throw new NullReferenceException(
                    $"{nameof(m_ServiceProvider)} is null. Have you called {nameof(IInjectionProxy)}.{nameof(Init)}() and {nameof(IInjectionProxy)}.{nameof(Complete)}()?");
            }

            return m_ServiceProvider.GetService(i_Type);
        }

        /// <exception cref="NullReferenceException">IInjectionProxy not initialized.</exception>
        public T Get<T>(bool i_Optional = false) 
            where T : class
        {
            if (m_ServiceProvider == null)
            {
                throw new NullReferenceException(
                    $"{nameof(m_ServiceProvider)} is null. Have you called {nameof(IInjectionProxy)}.{nameof(Init)}() and {nameof(IInjectionProxy)}.{nameof(Complete)}()?");
            }

            if (i_Optional && !IsRegistered<T>())
            {
                return null;
            }

            return m_ServiceProvider.GetService<T>();
        }

        private void register<T>(IRegistrationBuilder<T, IConcreteActivatorData, SingleRegistrationStyle> i_RegistrationBuilder, InstanceType i_InstanceType)
        {
            switch (i_InstanceType)
            {
                case InstanceType.EachResolve:
                    i_RegistrationBuilder.InstancePerDependency();
                    break;
                case InstanceType.SingleInstance:
                    i_RegistrationBuilder.SingleInstance();
                    break;
                default:
                    i_RegistrationBuilder.InstancePerDependency();
                    break;
            }
        }
    }
}