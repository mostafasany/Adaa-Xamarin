using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.Controls;
using AdaaMobile.Models;
using AdaaMobile.ViewModels;
using Xamarin.Forms;
using AdaaMobile.Strings;

namespace AdaaMobile.Views
{
    public partial class AttendancePage : ContentPage
    {
        private readonly AttendanceViewModel _attendanceViewModel;

        private Button _lastTappedTab;

        #region List Related fields
        private const string DayLabelName = "DayLabel";
        private const string MonthLabelName = "MonthLabel";
        private const string DayBackName = "DayBack";
        private const string DayBackNormalKey = "NormalBackColor";
        #endregion

        public AttendancePage()
        {
            InitializeComponent();
            _attendanceViewModel = Locator.Default.AttendanceViewModel;
            BindingContext = _attendanceViewModel;

            //Add App bar icon
            //Action action = () => { };
            //ToolbarItems.Add(
            //    new ToolbarItem("Refresh", "icon.png", action, ToolbarItemOrder.Default));

            //Select Tabs
            SelectButton(AttendanceButton, true);
            SelectButton(ExceptionsButton, false);
            _lastTappedTab = AttendanceButton;

			Title = "Attendance";

        }

        #region DatePickers
        private void EndDate_Clicked(object sender, EventArgs e)
        {
            MaxDatePicker.Focus();
        }

        private void StartDate_Clicked(object sender, EventArgs e)
        {
            MinDatePicker.Focus();
        }
        #endregion

        #region Tabs Methods
        private void SelectButton(Button button, bool selected)
        {
			var darkColor = (Color)Application.Current.Resources["PurpleAccent"];

            if (selected)
            {
                button.BackgroundColor = darkColor;
                button.TextColor = Color.White;
            }
            else
            {
                button.BackgroundColor = Color.White;
                button.TextColor = darkColor;
            }
        }

        /// <summary>
        /// This method changes button backgroundcolor and textcolor based on pressed state.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTabTapped(object sender, EventArgs e)
        {
            var button = (Button)sender;
            if (button == _lastTappedTab) return;
            //Change color state of tapped button to Active
            SelectButton(button, true);
            //Change color state of tapped button to be not Active
            if (_lastTappedTab != null)
            {
                SelectButton(_lastTappedTab, false);
            }
            _lastTappedTab = button;

            //Switch to different modes based on tapped button
            if (button == AttendanceButton && _attendanceViewModel.AttendanceMode != AttendanceMode.Attendance)
            {
                _attendanceViewModel.SwitchMode(AttendanceMode.Attendance);
            }
            else if (button == ExceptionsButton && _attendanceViewModel.AttendanceMode != AttendanceMode.Exceptions)
            {
                _attendanceViewModel.SwitchMode(AttendanceMode.Exceptions);
            }
        }

        #endregion

        /// <summary>
        /// Changes color state and triggers new load of details.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Day_Tapped(object sender, HorizontaListItemTappedEventArgs e)
        {
            //Change color states of clicked day
            var newDay = (DayWrapper)e.Item;
            newDay.IsSelected = true;
            var oldDay = _attendanceViewModel.SelectedDay;
            if (oldDay != null && oldDay != newDay)
                oldDay.IsSelected = false;
            _attendanceViewModel.SelectedDay = newDay;

            if (_attendanceViewModel.AttendanceMode == AttendanceMode.Attendance)
            {
                //Load Attendance details
                _attendanceViewModel.LoadAttendanceCommand.Execute(null);
            }
            else
            {
                //Binding is happening through Selected day
            }
        }

        private void Date_Selected(object sender, DateChangedEventArgs e)
        {
            //_attendanceViewModel.PopulateAttendanceDays();
        }
    }
}
