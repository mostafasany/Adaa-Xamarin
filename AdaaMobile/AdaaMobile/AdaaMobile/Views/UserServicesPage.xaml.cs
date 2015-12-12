using AdaaMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace AdaaMobile.Views
{
    public partial class UserServicesPage : ContentPage
    {
        private readonly UserAccountServicesViewModel _userServicesViewModel;

        public UserServicesPage()
        {
            InitializeComponent();
			_userServicesViewModel = Locator.Default.UserAccountServicesViewModel;
            BindingContext = _userServicesViewModel;


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
    }
}
