using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.ViewModels;
using Xamarin.Forms;
using AdaaMobile.Strings;

namespace AdaaMobile.Views
{
    public partial class AttendancePage : ContentPage
    {
        private readonly AttendanceViewModel _attendanceViewModel;
        private Grid _lastTappedGrid;
        private const string DayLabelName = "DayLabel";
        private const string MonthLabelName = "MonthLabel";
        private const string DayBackName = "DayBack";
        private const string DayBackNormalKey = "NormalBackColor";
        
        public AttendancePage()
        {
            InitializeComponent();
            _attendanceViewModel = Locator.Default.AttendanceViewModel;
            BindingContext = _attendanceViewModel;
            Action action = () => { };
            ToolbarItems.Add(
                new ToolbarItem("Refresh", "icon.png", action, ToolbarItemOrder.Default));
        }


        private void Day_Tapped(object sender, EventArgs e)
        {
            var tappedGrid = (Grid)sender;
            //Set selected state
            SetTextColor(tappedGrid, DayLabelName, (Color)App.Current.Resources["YellowAccent"]);
            SetTextColor(tappedGrid, MonthLabelName, Color.White);
            SetBackColor(tappedGrid, DayBackName, (Color)App.Current.Resources["AppBackgroundLight"]);

            //Reset to normal state
            if (_lastTappedGrid != null)
            {
                SetTextColor(_lastTappedGrid, DayLabelName, Color.Black);
                SetTextColor(_lastTappedGrid, MonthLabelName, Color.Black);
                SetBackColor(_lastTappedGrid, DayBackName, (Color)Resources[DayBackNormalKey]);

            }

            //Assign last tapped grid
            _lastTappedGrid = tappedGrid;
        }

        private void SetTextColor(Grid grid, string labelName, Color color)
        {
            var label = grid.FindByName<Label>(labelName);
            if (label != null)
            {
                label.TextColor = color;
            }
        }

        private void SetBackColor(Grid grid, string boxName, Color color)
        {
            var boxView = grid.FindByName<BoxView>(boxName);
            if (boxView != null)
            {
                boxView.BackgroundColor = color;
            }
        }
    }
}
