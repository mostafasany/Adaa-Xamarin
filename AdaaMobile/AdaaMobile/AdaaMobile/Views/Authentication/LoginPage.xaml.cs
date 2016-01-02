using AdaaMobile.ViewModels;
using Autofac;
using Xamarin.Forms;

namespace AdaaMobile.Views.Authentication
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = Locator.Container.Resolve<LoginViewModel>(
                new TypedParameter(typeof(INavigation), Navigation));

            LoginBtn.FontFamily = Device.OnPlatform(
                "ProximaNova-Semibold",
                null,
                null
            );



        }
    }
}
