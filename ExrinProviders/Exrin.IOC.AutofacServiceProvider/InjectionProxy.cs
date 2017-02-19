using System;
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
        }

        public bool IsRegistered<T>()
        {
            return m_Container.IsRegistered<T>();
        }

        public void Register<T>(InstanceType i_InstanceType = InstanceType.SingleInstance) where T : class
        {
            register(r_Builder.RegisterType<T>(), i_InstanceType);
        }

        public void RegisterInterface<TInterface, TType>(InstanceType i_InstanceType = InstanceType.SingleInstance) 
            where TInterface : class 
            where TType : class, TInterface
        {
            register(r_Builder.RegisterType<Type>().As<TInterface>(), i_InstanceType);
        }

        public void RegisterInstance<TInterface, TType>(TType i_Instance) 
            where TInterface : class 
            where TType : class, TInterface
        {
            r_Builder.RegisterInstance<TType>(i_Instance).As<TInterface>().SingleInstance();
        }

        public void RegisterInstance<T>(T i_Instance) 
            where T : class
        {
            r_Builder.RegisterInstance(i_Instance).As<T>().SingleInstance();
        }

        public object Get(Type i_Type)
        {
            return m_ServiceProvider.GetService(i_Type);
        }

        public T Get<T>(bool i_Optional = false) 
            where T : class
        {
            if(i_Optional && !IsRegistered<T>())
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