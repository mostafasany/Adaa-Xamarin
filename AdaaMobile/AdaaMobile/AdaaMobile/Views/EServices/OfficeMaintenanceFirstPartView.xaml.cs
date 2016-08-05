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
			HandleArabicLanguageFlowDirection ();
        }

		void HandleArabicLanguageFlowDirection()
		{
			if (Locator.Default.AppSettings.SelectedCultureName.Contains("ar"))
			{
				//Equipment
				lblTypeOfEquipment.HorizontalOptions = LayoutOptions.End;
				lblTypeOfEquipmentResult.HorizontalOptions = LayoutOptions.End;
				Grid.SetColumn (imgTypeOfEquipmentResult, 0);
				imgTypeOfEquipmentResult.RotationY = 180;
			
				//Location
				lblLocation.HorizontalOptions = LayoutOptions.End;
				lblLocationResult.HorizontalOptions = LayoutOptions.End;
				Grid.SetColumn (imgLocation, 0);
				imgLocation.RotationY = 180;

				//Rooms
				lblRoom.HorizontalOptions = LayoutOptions.End;
				lblRoomResult.HorizontalOptions = LayoutOptions.End;
				Grid.SetColumn (imgRoom, 0);
				imgRoom.RotationY = 180;

				//Priority
				lblPriority.HorizontalOptions = LayoutOptions.End;
				lblPriorityResult.HorizontalOptions = LayoutOptions.End;
				Grid.SetColumn (imgsPriority, 0);
				imgsPriority.RotationY = 180;

			}
		}


        //This will be called from view model to update room selection when rooms are selected.
        private void ReflectRoomSelection()
        {
            if (ViewModel.Rooms == null || ViewModel.Rooms.Length == 0 || ViewModel.SelectedRoom == null)
            {
                RoomPicker.SelectedIndex = -1;
                return;
            }

            RoomPicker.SelectedIndex = Array.IndexOf(ViewModel.Rooms, ViewModel.SelectedRoom);
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

            //This will be called from view model to update room selection when rooms are selected.
            ViewModel.ReflectRoomSelection = ReflectRoomSelection;
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
