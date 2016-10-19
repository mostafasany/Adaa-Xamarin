using Xamarin.Forms;
using AdaaMobile.ViewModels;
using AdaaMobile.Strings;
using System.Linq;

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

            Title = AppResources.RequestDetailsCap;
        }

        protected async override void OnAppearing()
        {
            var attchments = await Locator.Default.DataService.GetServiceDeskRequestsAttachments(true, _viewModel.SelectedRequests.ID, null);
            if (attchments.ResponseStatus == DataServices.Requests.ResponseStatus.SuccessWithResult)
                _viewModel.Attatchments = new System.Collections.ObjectModel.ObservableCollection<Models.Response.Attatchment>(attchments.Result.result.Table1.ToList());
        }
    }
}

