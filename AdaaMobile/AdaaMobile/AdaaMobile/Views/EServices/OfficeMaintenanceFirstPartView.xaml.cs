using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.Models;
using AdaaMobile.Models.Response;
using AdaaMobile.Strings;
using AdaaMobile.ViewModels;
using Xamarin.Forms;

namespace AdaaMobile.Views.EServices
{
    public partial class OfficeMaintenanceFirstPartView : ContentView
    {
        private OfficeMaintenanceViewModel ViewModel
        {
            get { return BindingContext as OfficeMaintenanceViewModel; }
        }

        public OfficeMaintenanceFirstPartView()
        {
            InitializeComponent();
        }

        private void ChooseLocation_Tapped(object sender, EventArgs e)
        {
            OfficeLocationPicker.Unfocus();
            OfficeLocationPicker.Focus();
        }

        private async void ChooseRoom_Tapped(object sender, EventArgs e)
        {
            if (ViewModel.SelectedLocation == null)
            {
                await Locator.Default.DialogManager.DisplayAlert(AppResources.ApplicationName, AppResources.SelectOfficleLocationFirst, AppResources.Ok);
                return;
            }
            RoomPicker.Unfocus();
            RoomPicker.Focus();
        }

        private void ChoosePriority_Tapped(object sender, EventArgs e)
        {
            PriorityPicker.Unfocus();
            PriorityPicker.Focus();
        }

        private void RoomPicker_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var list = ViewModel.Rooms;
            int index = RoomPicker.SelectedIndex;
            if (list == null || index == -1) return;
            ViewModel.SelectedRoom = list[index];
        }

        private void OfficeLocationPicker_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var list = ViewModel.Locations;
            int index = OfficeLocationPicker.SelectedIndex;
            if (list == null || index == -1) return;
            ViewModel.SelectedLocation = list[index];
        }

        private void PriorityPicker_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var list = ViewModel.Priorities;
            int index = PriorityPicker.SelectedIndex;
            if (list == null || index == -1) return;
            ViewModel.SelectedPriority = list[index];
        }
    }
}
