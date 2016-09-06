using AdaaMobile.Strings;
using AdaaMobile.ViewModels;
using Xamarin.Forms;

namespace AdaaMobile
{
    public partial class ServiceDeskCasesPage : ContentPage
    {
        //ServiceDeskRequestsViewModel _viewModel;
        public ServiceDeskCasesPage()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");

            //_viewModel = Locator.Default.ServiceDeskRequestsViewModel;
            //BindingContext = _viewModel;

            Title = AppResources.ServiceDesk_MyRequests;

        }

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
			//_viewModel.PageLoadedCommand.Execute (null);
		}
    }
}
