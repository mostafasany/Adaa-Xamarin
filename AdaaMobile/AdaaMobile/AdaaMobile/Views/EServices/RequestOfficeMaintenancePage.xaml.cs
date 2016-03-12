using AdaaMobile.Strings;
using Xamarin.Forms;

namespace AdaaMobile.Views.EServices
{
	public partial class RequestOfficeMaintenancePage : ContentPage
	{
		public RequestOfficeMaintenancePage ()
		{
			InitializeComponent ();
			NavigationPage.SetBackButtonTitle(this, "");

//			_viewModel = Locator.Default.EServicesViewModel;
//			BindingContext = _viewModel;

			Title = AppResources.RequestOfficeMaintenance;
		}
	}
}

