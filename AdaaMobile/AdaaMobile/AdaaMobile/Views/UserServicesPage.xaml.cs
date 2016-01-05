using AdaaMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.Strings;
using Xamarin.Forms;

namespace AdaaMobile.Views
{
    public partial class UserServicesPage : ContentPage
    {
        private readonly UserAccountServicesViewModel _userServicesViewModel;

        public UserServicesPage()
        {
            InitializeComponent();

			NavigationPage.SetBackButtonTitle(this, "");
            _userServicesViewModel = Locator.Default.UserAccountServicesViewModel;
            BindingContext = _userServicesViewModel;

			Title = AppResources.UserAccountServices;
            //Work-around for iOS for cut-images
            if (Device.OS == TargetPlatform.iOS)
            {
                iosBackgroundImage.IsVisible = true;
            }
            else
            {
                iosBackgroundImage.IsVisible = false;
            }

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _userServicesViewModel.LoadCommand.Execute(null);
        }
    }
}
