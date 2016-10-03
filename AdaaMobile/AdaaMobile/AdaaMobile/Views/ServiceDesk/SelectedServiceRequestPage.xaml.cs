using Xamarin.Forms;
using AdaaMobile.ViewModels;

namespace AdaaMobile
{
    public partial class SelectedServiceRequestPage : ContentPage
    {
        ServiceDeskRequestsViewModel _viewModel;

        public SelectedServiceRequestPage()
        {
            InitializeComponent();

            NavigationPage.SetBackButtonTitle(this, "");

            _viewModel = Locator.Default.ServiceDeskRequestsViewModel;
            BindingContext = _viewModel;

            Title = _viewModel.SelectedRequests.Title;
        }
    }
}

