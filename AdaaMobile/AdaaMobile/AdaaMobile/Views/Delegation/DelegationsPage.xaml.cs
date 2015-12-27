using System;
using AdaaMobile.ViewModels;
using Xamarin.Forms;

namespace AdaaMobile.Views.Delegation
{
    public partial class DelegationsPage : ContentPage
    {
        private DelegationViewModel _delegationViewModel;
        public DelegationsPage()
        {
            InitializeComponent();
            _delegationViewModel = Locator.Default.DelegationViewModel;
            BindingContext = _delegationViewModel;
            Title = "My Delegations";

            Action action = () =>
            {
                this.Navigation.PushAsync(new NewDelegationPage());
            };
            ToolbarItems.Add(
                new ToolbarItem("Add", "", action, ToolbarItemOrder.Primary));

            DelegationsList.ItemTapped += DelegationsList_ItemTapped;
        }

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
			_delegationViewModel.LoadDayPassDataCommand.Execute (null);
		}
        private void DelegationsList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Models.Response.Delegation delegation = (e.Item as Models.Response.Delegation);
            this.Navigation.PushAsync(new DelegationDetailsPage(delegation));
        }
    }
}
