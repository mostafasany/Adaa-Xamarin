using AdaaMobile.ViewModels;
using Xamarin.Forms;

namespace AdaaMobile.Views.Delegation
{
    public partial class DelegationDetailsPage : ContentPage
    {
        private Models.Response.Delegation delegation;
        private DelegationViewModel _delegationViewModel;

        public DelegationDetailsPage(Models.Response.Delegation delegation)
        {
            InitializeComponent();
            _delegationViewModel = Locator.Default.DelegationViewModel;
            BindingContext = _delegationViewModel;
            this.delegation = delegation;
            Title = "Delegation details";
            DelegateNameLabel.Text = delegation.DelegateName;
            SubordinateLabel.Text = delegation.SubordinateName;
        }
    }
}
