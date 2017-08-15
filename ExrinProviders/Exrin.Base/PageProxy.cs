using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exrin.Abstraction;
using Xamarin.Forms;

namespace Exrin.Base
{
    /// <summary>
    /// Implements IView on all ContentPages. ALL your views must inherit from this proxy.
    /// </summary>
    public class PageProxy : ContentPage, IView
    {
        protected override bool OnBackButtonPressed()
        {
            return ((IView)this).OnBackButtonPressed();
        }

        Func<bool> IView.OnBackButtonPressed { get; set; }
    }
}
