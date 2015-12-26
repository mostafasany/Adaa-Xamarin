using AdaaMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace AdaaMobile.Views
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
