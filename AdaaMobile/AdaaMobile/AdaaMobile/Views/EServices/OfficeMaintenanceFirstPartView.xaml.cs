using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace AdaaMobile.Views.EServices
{
    public partial class OfficeMaintenanceFirstPartView : ContentView
    {
        public OfficeMaintenanceFirstPartView()
        {
            InitializeComponent();
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
