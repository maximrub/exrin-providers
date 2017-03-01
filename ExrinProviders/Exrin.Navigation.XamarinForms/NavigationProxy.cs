using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Exrin.Abstraction;
using Exrin.Framework;
using Xamarin.Forms;

namespace Exrin.Navigation.XamarinForms
{
    public class NavigationProxy : INavigationProxy
    {
        private readonly Queue<object> r_ParameterQueue;
        private NavigationPage m_Page;

        public NavigationProxy(NavigationPage i_Page)
        {
            r_ParameterQueue = new Queue<object>();
            ViewStatus = VisualStatus.Unseen;
            m_Page = i_Page;
            m_Page.Popped += r_Page_Popped;
        }

        public event EventHandler<IViewNavigationArgs> OnPopped;

        public object NativeView
        {
            get
            {
                return m_Page;
            }
        }

        public VisualStatus ViewStatus { get; set; }

        public void SetNavigationBar(bool i_IsVisible, object i_Page)
        {
            BindableObject bindableObject = i_Page as BindableObject;
            if(bindableObject != null)
            {
                NavigationPage.SetHasNavigationBar(bindableObject, i_IsVisible);
            }
        }

        public bool CanGoBack()
        {
            return m_Page.Navigation.NavigationStack.Count > 1;
        }

        public async Task PopAsync()
        {
            await m_Page.PopAsync();
        }

        public async Task PopAsync(object i_Parameter)
        {
            r_ParameterQueue.Enqueue(i_Parameter);
            await m_Page.PopAsync();
        }

        public Task SilentPopAsync(int i_IndexFromTop)
        {
            m_Page.Navigation.RemovePage(m_Page.Navigation.NavigationStack[m_Page.Navigation.NavigationStack.Count - i_IndexFromTop - 1]);

            return Task.FromResult(true);
        }

        /// <exception cref="Exception">only Xamarin Page can be pushed</exception>
        public async Task PushAsync(object i_Page)
        {
            var xamarinPage = i_Page as Page;
            if(xamarinPage == null)
            {
                throw new Exception("only Xamarin Page can be pushed");
            }

            await m_Page.PushAsync(xamarinPage);
        }

        public Task ClearAsync()
        {
            m_Page = new NavigationPage();

            return Task.FromResult(true);
        }

        /// <exception cref="Exception">Cannot show dialog on a non-visible page</exception>
        public async Task ShowDialog(IDialogOptions i_DialogOptions)
        {
            if(ViewStatus != VisualStatus.Visible)
            {
                throw new Exception("Cannot show dialog on a non-visible page");
            }

            await m_Page.DisplayAlert(i_DialogOptions.Title, i_DialogOptions.Message, "OK");
            i_DialogOptions.Result = true;
        }

        protected virtual void OnPagePopped(Page i_PoppedPage)
        {
            if (OnPopped != null)
            {
                ViewNavigationArgs viewNavigationArgs = new ViewNavigationArgs();
                viewNavigationArgs.PoppedView = i_PoppedPage as IView;
                viewNavigationArgs.CurrentView = m_Page.CurrentPage as IView;
                viewNavigationArgs.Parameter = r_ParameterQueue.Count > 0 ? r_ParameterQueue.Dequeue() : null;
                OnPopped.Invoke(this, viewNavigationArgs);
            }
        }

        private void r_Page_Popped(object i_Sender, NavigationEventArgs i_EventArgs)
        {
            OnPagePopped(i_EventArgs.Page);
        }
    }
}
