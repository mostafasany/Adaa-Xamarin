using AdaaMobile.Strings;
using AdaaMobile.ViewModels;
using Xamarin.Forms;

namespace AdaaMobile.Views.EServices
{
    public partial class EServicesPage : ContentPage
    {
        private readonly EServicesViewModel _viewModel;

        public EServicesPage()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");

            _viewModel = Locator.Default.EServicesViewModel;
            BindingContext = _viewModel;

            Title = AppResources.EServices;

        }



        private void ServicesList_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (ServicesList.SelectedItem != null)
                ServicesList.SelectedItem = null;
        }
    }
}

