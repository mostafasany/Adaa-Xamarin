using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.Constants;
using AdaaMobile.Models.Response;
using AdaaMobile.Strings;
using AdaaMobile.ViewModels;
using Xamarin.Forms;

namespace AdaaMobile.Views.EServices
{
    public partial class EquipmentsPage : ContentPage
    {
        private readonly EquipmentsSelectionViewModel _viewModel;
        public EquipmentsPage(Equipment[] allEquipments, List<Equipment> previousSelection)
        {
            InitializeComponent();
            Title = AppResources.TypeOfEquipment;
            this._viewModel = Locator.Default.EquipmentsSelectionViewModel;
            _viewModel.InitializeWrappersList(allEquipments, previousSelection);
            BindingContext = _viewModel;
        }

        protected override bool OnBackButtonPressed()
        {

            return base.OnBackButtonPressed();
        }

        protected override void OnDisappearing()
        {
            MessagingCenter.Send<object, List<Equipment>>(this, MessagingConstants.SelectedEquipments, _viewModel.GetSelectedEquipments());
            base.OnDisappearing();
        }

        private void EquipmentsList_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (EquipmentsList.SelectedItem != null)
                EquipmentsList.SelectedItem = null;
        }
    }
}
