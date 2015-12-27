using AdaaMobile.Strings;
using AdaaMobile.ViewModels;
using Xamarin.Forms;

namespace AdaaMobile.Views.Delegation
{
    public partial class DelegationDetailsPage : ContentPage
    {
        private readonly DelegationDetailsViewModel _delegationDetailsViewModel;

        public DelegationDetailsPage(Models.Response.Delegation delegation)
        {
            InitializeComponent();
            Title = AppResources.DelegationDetailsCap;
            _delegationDetailsViewModel = Locator.Default.DelegationDetailsViewModel;
            _delegationDetailsViewModel.Delegation = delegation;
            BindingContext = _delegationDetailsViewModel;

        }
    }
}
