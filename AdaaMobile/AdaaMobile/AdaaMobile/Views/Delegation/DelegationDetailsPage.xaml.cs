using AdaaMobile.Strings;
using AdaaMobile.ViewModels;
using Xamarin.Forms;

namespace AdaaMobile.Views.Delegation
{
	public partial class DelegationDetailsPage : ContentPage
	{
		private readonly DelegationDetailsViewModel _delegationDetailsViewModel;

		public DelegationDetailsPage ()
		{
			InitializeComponent ();
			NavigationPage.SetBackButtonTitle (this, "");
			Models.Response.Delegation delegation = Locator.Default.DelegationViewModel.SelectedDelegation;
			Title = AppResources.DelegationDetailsCap;
			_delegationDetailsViewModel = Locator.Default.DelegationDetailsViewModel;
			_delegationDetailsViewModel.Delegation = delegation;
			BindingContext = _delegationDetailsViewModel;

		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
			NavigationPage.SetBackButtonTitle (this, "sdfsdf");

		}
	}
}
