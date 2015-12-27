using System;
using AdaaMobile.Strings;
using AdaaMobile.ViewModels;
using Xamarin.Forms;

namespace AdaaMobile.Views.Delegation
{
    public partial class NewDelegationPage : ContentPage
    {
        private NewDelegationViewModel _newDelegationViewModel;

        public NewDelegationPage()
        {
            InitializeComponent();
            _newDelegationViewModel = Locator.Default.NewDelegationViewModel;
            BindingContext = _newDelegationViewModel;
            Title = AppResources.NewDelegationCap;
            Action action = () =>
            {
                _newDelegationViewModel.NewDelegationCommand.Execute(null);
            };
            ToolbarItems.Add(
                new ToolbarItem(AppResources.Send, "", action, ToolbarItemOrder.Primary));

        }
    }
}
