using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.ViewModels;
using Autofac;
using Xamarin.Forms;

namespace AdaaMobile.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = Locator.Container.Resolve<LoginViewModel>(
                new TypedParameter(typeof(INavigation), Navigation));
   
			LoginBtn.FontFamily = Device.OnPlatform (
				"ProximaNova-Semibold",
				null,
				null
			);


        }
    }
}
