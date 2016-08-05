using AdaaMobile.Models;
using AdaaMobile.Models.Response;
using AdaaMobile.Strings;
using Xamarin.Forms;
using AdaaMobile.ViewModels;

namespace AdaaMobile.Views.DayPass
{
	public partial class DayPassRequestDetailsPage : ContentPage
	{
		private DayPassRequest request;


		public DayPassRequestDetailsPage ()
		{
			InitializeComponent ();
			NavigationPage.SetBackButtonTitle (this, "");
			var _viewModel = Locator.Default.DayPassViewModel;
			var request = _viewModel.SelectedRequest;

			this.request = request;
			this.BindingContext = request;
			Title = AppResources.RequestDetails;

		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
			NavigationPage.SetBackButtonTitle (this, string.Empty);
		}
	}
}
