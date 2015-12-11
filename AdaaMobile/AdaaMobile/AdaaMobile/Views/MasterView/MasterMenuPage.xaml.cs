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
        }

        private void SideMenuItemControl_OnTapped(object sender, EventArgs e)
        {
            
            //OnOnItemTapped();//This will be used in Master details page to set IsPresented to false
            var masterDetailPage = Application.Current.MainPage as MasterDetailPage;
            if (masterDetailPage != null) masterDetailPage.IsPresented = false;

            var selectedControl = sender as SideMenuItemControl;
            
            if (selectedControl == null || !(selectedControl.BindingContext is Type)) return;
            //AnimateControl(selectedControl);
            if (!selectedControl.IsPageSupported)
            {
                return;//Page is not supported in this current release build
            }
            Type pageType = selectedControl.BindingContext as Type;

            //Update selected item color
            if (_lastSelectedControl != null)
                _lastSelectedControl.IsSelected = false;
            //Update lastSelected item color
            selectedControl.IsSelected = true;
            //set last selected to item
            _lastSelectedControl = selectedControl;
            Locator.Default.NavigationService.SetMasterDetailsPage(pageType);
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
