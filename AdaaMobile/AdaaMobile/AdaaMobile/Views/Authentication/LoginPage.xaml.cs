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

            if (Locator.Default.AppSettings.SelectedCultureName.Contains("ar"))
            {
                ShowPasswordLabel.HorizontalOptions = LayoutOptions.End;
                PasswordToggle.HorizontalOptions = LayoutOptions.Start;
            }

        }

        private void ShowPassword_Toggled(object sender, ToggledEventArgs e)
        {
            PasswordEntry.IsPassword = !e.Value;
        }
    }
}
