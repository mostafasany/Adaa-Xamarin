using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.Models;
using AdaaMobile.ViewModels;
using Xamarin.Forms;

namespace AdaaMobile.Views.MasterView
{
    public partial class MasterMenuPage : ContentPage
    {
        private SideMenuItemControl _lastSelectedControl;

        public MasterMenuPage()
        {
            InitializeComponent();

            _lastSelectedControl = HomeControl;
            _lastSelectedControl.IsSelected = true;
            Icon = "menu.png";

            if (Device.OS == TargetPlatform.iOS)
                CrosspondencePage.IsPageSupported = true;
            else
                CrosspondencePage.IsPageSupported = false;
        }

        private void SideMenuItemControl_OnTapped(object sender, EventArgs e)
        {

            var masterDetailPage = Application.Current.MainPage as MasterDetailPage;

            //Hide Master(Menu) when user clicks
            if (masterDetailPage != null) masterDetailPage.IsPresented = false;

            var selectedControl = sender as SideMenuItemControl;
            if (selectedControl == null || !(selectedControl.BindingContext is Type)) return;


            if (!selectedControl.IsPageSupported)
            {
                return;//Page is not supported in this current release build
            }

            Type pageType = selectedControl.BindingContext as Type;
            //Update color selection
            UpdateSelectedMenu(selectedControl);

            if (pageType == typeof(MyTasksPage))
            {
                //Open the app
                DependencyService.Get<IPhoneService>().OpenCrosspondenceApp();

                return;
            }


            //Now navigate to the target page.
            Locator.Default.NavigationService.SetMasterDetailsPage(pageType);
        }

        /// <summary>
        /// This method will loop on all children of Menu stack until it finds
        /// Side menu control which represents this page type.
        /// </summary>
        /// <param name="pageType"></param>
        /// <returns></returns>
        public SideMenuItemControl GetSideMenuOfType(Type pageType)
        {
            return GetSideMenuOfType(MenuStack, pageType);
        }

        /// <summary>
        /// This method will loop on all children of this layout<view> until it finds
        /// Side menu control which represents this page type.
        /// </summary>
        /// <param name="layout"></param>
        /// <param name="pageType"></param>
        /// <returns></returns>
        private SideMenuItemControl GetSideMenuOfType(Layout<View> layout, Type pageType)
        {
            foreach (var child in layout.Children)
            {
                if (child is SideMenuItemControl)
                {
                    if ((Type)child.BindingContext == pageType)
                        return (SideMenuItemControl)child;
                }
                else if (child is Layout<View>)
                {
                    var result = GetSideMenuOfType(child as Layout<View>, pageType);
                    if (result != null) return result;
                }
            }
            return null;
        }

        /// <summary>
        /// This method will find the side menu that represents this page type,
        /// Then it will select it.
        /// </summary>
        /// <param name="pageType"></param>
        public void UpdateSelectedMenu(Type pageType)
        {
            UpdateSelectedMenu(GetSideMenuOfType(MenuStack, pageType));
        }

        private void UpdateSelectedMenu(SideMenuItemControl selectedControl)
        {
            //Update selected item color
            if (_lastSelectedControl != null)
                _lastSelectedControl.IsSelected = false;
            //Update lastSelected item color
            if (selectedControl != null)
                selectedControl.IsSelected = true;
            //set last selected to item
            _lastSelectedControl = selectedControl;
        }

        private async void AnimateControl(SideMenuItemControl selectedControl)
        {
            selectedControl.Opacity = .5;
            await Task.Delay(400);
            selectedControl.Opacity = 1;
        }

        public event EventHandler ItemTapped;

        protected virtual void OnOnItemTapped()
        {
            var handler = ItemTapped;
            if (handler != null) handler.Invoke(this, EventArgs.Empty);
        }
    }
}
