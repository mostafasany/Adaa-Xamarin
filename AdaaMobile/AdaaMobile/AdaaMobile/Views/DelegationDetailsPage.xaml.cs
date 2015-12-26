using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.Models;
using Xamarin.Forms;
using AdaaMobile.ViewModels;

namespace AdaaMobile.Views
{
    public partial class DelegationDetailsPage : ContentPage
    {
        private Delegation delegation;
        private DelegationViewModel _delegationViewModel;

        public DelegationDetailsPage(Delegation delegation)
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
