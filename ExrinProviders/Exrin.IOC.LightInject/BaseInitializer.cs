using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightInject;

namespace Exrin.IOC.LightInject
{
    public abstract class BaseInitializer
    {
        public event EventHandler<ServiceContainer> Initialized;

        /// <summary>
        /// Register Components using LightInject container
        /// </summary>
        protected internal virtual void Register(ServiceContainer i_Container)
        {
        }

        protected internal virtual void OnInitialized(ServiceContainer i_Container)
        {
            if (Initialized != null)
            {
                Initialized.Invoke(this, i_Container);
            }
        }
    }
}
