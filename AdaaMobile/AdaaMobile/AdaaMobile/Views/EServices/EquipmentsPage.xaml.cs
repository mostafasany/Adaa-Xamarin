using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.Models.Response;
using AdaaMobile.ViewModels;
using Xamarin.Forms;

namespace AdaaMobile.Views.EServices
{
    public partial class EquipmentsPage : ContentPage, IEquipmentsSelection
    {
        private readonly EquipmentsSelectionViewModel _viewModel;
        public EquipmentsPage(Equipment[] allEquipments)
        {
            this._viewModel = Locator.Default.EquipmentsSelectionViewModel;
            _viewModel.InitializeWrappersList(allEquipments);
            BindingContext = _viewModel;
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {

            return base.OnBackButtonPressed();
        }

        protected override void OnDisappearing()
        {
            OnEquipmentsSelected(_viewModel.GetSelectedEquipments());
            base.OnDisappearing();
        }

        public event EventHandler<List<Equipment>> EquipmentsSelected;

        protected virtual void OnEquipmentsSelected(List<Equipment> e)
        {
            var handler = EquipmentsSelected;
            if (handler != null) handler?.Invoke(this, e);
        }
    }
}
