using System;
using AdaaMobile.Strings;
using AdaaMobile.ViewModels;
using Xamarin.Forms;

namespace AdaaMobile.Views.EServices
{
    public partial class RequestOfficeMaintenancePage : ContentPage
    {
        public RequestOfficeMaintenancePage()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");
            Title = AppResources.RequestOfficeMaintenance;

            var viewModel = Locator.Default.OfficeMaintenanceViewModel;
            BindingContext = viewModel;

        }


        private void ChooseLocation_Tapped(object sender, EventArgs e)
        {
           OfficeLocationPicker.Unfocus();
           OfficeLocationPicker.Focus();
        }

        private void ChooseRoom_Tapped(object sender, EventArgs e)
        {
            RoomPicker.Unfocus();
            RoomPicker.Focus();
        }

        private void ChoosePriority_Tapped(object sender, EventArgs e)
        {
            PriorityPicker.Unfocus();
            PriorityPicker.Focus();
        }
    }
}

