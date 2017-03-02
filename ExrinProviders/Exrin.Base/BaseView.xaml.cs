using System;
using Exrin.Abstraction;
using Xamarin.Forms;

namespace Exrin.Base
{
    public partial class BaseView : ContentPage, IView
    {
        public BaseView()
        {
            InitializeComponent();
        }

        Func<bool> IView.OnBackButtonPressed { get; set; }

        protected override bool OnBackButtonPressed()
        {
            return ((IView)this).OnBackButtonPressed();
        }
    }
}