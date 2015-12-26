using AdaaMobile.Models;
using AdaaMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace AdaaMobile.Views
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

        private void DelegationsList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Delegation delegation = (e.Item as Delegation);
            this.Navigation.PushAsync(new DelegationDetailsPage(delegation));
        }
    }
}
