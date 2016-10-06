using Xamarin.Forms;
using AdaaMobile.ViewModels;
using AdaaMobile.Strings;

namespace AdaaMobile
{
	public partial class SelectedServiceCasesPage : ContentPage
	{
		ServiceDeskCasesViewModel _viewModel;

		public SelectedServiceCasesPage()
		{
			InitializeComponent();

			NavigationPage.SetBackButtonTitle(this, "");

			_viewModel = Locator.Default.ServiceDeskCasesViewModel;
			BindingContext = _viewModel;

			Title = AppResources.ServiceDeskCases_CaseDetails;
		}
	}
}

