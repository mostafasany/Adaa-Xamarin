using Xamarin.Forms;
using AdaaMobile.ViewModels;
using AdaaMobile.Strings;

namespace AdaaMobile
{
    public partial class ServiceDeskHomePage : ContentPage
    {
        private readonly ServiceDeskHomeViewModel _viewModel;
        public ServiceDeskHomePage()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");

            _viewModel = Locator.Default.ServiceDeskHomeViewModel;
            BindingContext = _viewModel;

            Title = AppResources.ServiceDesk_HomePagePageTitle;
        }

        private void ServicesList_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (ServicesList.SelectedItem != null)
                ServicesList.SelectedItem = null;
        }
    }
}

