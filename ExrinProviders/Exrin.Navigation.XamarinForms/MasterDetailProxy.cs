using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exrin.Abstraction;
using Xamarin.Forms;

namespace Exrin.Navigation.XamarinForms
{
    public class MasterDetailProxy : IMasterDetailProxy
    {
        private readonly MasterDetailPage r_MasterPage;

        public MasterDetailProxy(MasterDetailPage i_MasterPage)
        {
            View = i_MasterPage;
            r_MasterPage = i_MasterPage;
        }

        public object DetailNativeView
        {
            get
            {
                return r_MasterPage.Detail;
            }

            set
            {
                r_MasterPage.Detail = value as Page;
            }
        }

        public object MasterNativeView
        {
            get
            {
                return r_MasterPage.Master;
            }

            set
            {
                Page page = value as Page;
                if (string.IsNullOrEmpty(page.Title))
                {
                    page.Title = "Menu";
                }
                
                r_MasterPage.Master = page;
            }
        }

        public object View { get; set; }
    }
}
