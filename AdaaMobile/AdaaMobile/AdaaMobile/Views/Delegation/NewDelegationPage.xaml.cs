using System;
using AdaaMobile.ViewModels;
using Xamarin.Forms;

namespace AdaaMobile.Views.Delegation
{
    public partial class NewDelegationPage : ContentPage
    {
        private DelegationViewModel _delegationViewModel;

        public NewDelegationPage()
        {
            InitializeComponent();
            _delegationViewModel = Locator.Default.DelegationViewModel;
            BindingContext = _delegationViewModel;
            Title = "New Delegation";
            Action action = () =>
            {
                _delegationViewModel.NewDelegationCommand.Execute(null);
            };
            ToolbarItems.Add(
                new ToolbarItem("Send", "", action, ToolbarItemOrder.Primary));

        }
    }
}
