using System;
using Exrin.Abstraction;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Exrin.Base
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
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