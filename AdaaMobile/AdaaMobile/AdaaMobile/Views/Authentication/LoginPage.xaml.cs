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

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            //var client = new System.Net.Http.HttpClient(new ModernHttpClient.NativeMessageHandler());
            //var url = "https://adaamobile.adaa.abudhabi.ae/proxyservice/proxy?server=adaamobile&url=?funcname=getAllEmployeesList&langid=eng&userToken=2015112823041470554";
            //var data = await client.GetAsync(new System.Uri(url, System.UriKind.RelativeOrAbsolute));
        }
    }
}
