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
using Xamarin.Forms.Xaml;
using AdaaMobile.Models.Response;

namespace AdaaMobile.Views

{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SubAttendancePage : ContentPage
    {
        private readonly AttendanceViewModel _attendanceViewModel;

        private Button _lastTappedTab;

        #region List Related fields
        private const string DayLabelName = "DayLabel";
        private const string MonthLabelName = "MonthLabel";
        private const string DayBackName = "DayBack";
        private const string DayBackNormalKey = "NormalBackColor";
        #endregion

        public SubAttendancePage()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");
            _attendanceViewModel = Locator.Default.AttendanceViewModel;
            BindingContext = _attendanceViewModel;

            //TO Avoid that Date is fired on iOS before pressing Done
            MinDatePicker.Date = _attendanceViewModel.StartDate;
            MinDatePicker.Unfocused += MinDatePicker_Unfocused;

            MaxDatePicker.Date = _attendanceViewModel.EndDate;
            MaxDatePicker.Unfocused += MaxDatePicker_Unfocused;
            //Add App bar icon
            //Action action = () => { };
            //ToolbarItems.Add(
            //    new ToolbarItem("Refresh", "icon.png", action, ToolbarItemOrder.Default));

            //Select Tabs
            SelectButton(AttendanceButton, true);
            SelectButton(ExceptionsButton, false);
            _lastTappedTab = AttendanceButton;



            Title = AppResources.MySubAttendance;
			
            _attendanceViewModel.PropertyChanged += _attendanceViewModel_PropertyChanged;
        }


        void _attendanceViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "EndDate")
            {
                MaxDatePicker.Date = _attendanceViewModel.EndDate;

            }
            else if (e.PropertyName == "StartDate")
            {
                MinDatePicker.Date = _attendanceViewModel.StartDate;
            }
        }




        void MaxDatePicker_Unfocused(object sender, FocusEventArgs e)
        {
            DatePicker datePicker = (sender as DatePicker);
            if (datePicker != null && _attendanceViewModel != null)
                _attendanceViewModel.EndDate = datePicker.Date;
        }

        void MinDatePicker_Unfocused(object sender, FocusEventArgs e)
        {
            DatePicker datePicker = (sender as DatePicker);
            if (datePicker != null && _attendanceViewModel != null)
                _attendanceViewModel.StartDate = datePicker.Date;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            NavigationPage.SetBackButtonTitle(this, "");
            try
            {
                await _attendanceViewModel.PopulateAttendanceDaysAsync();

                if (_attendanceViewModel.CurrentAttendance == null)
                {
                    if (_attendanceViewModel.SubordinateList == null)
                    {
                        bool isHaveSubordinates = await _attendanceViewModel.GetSubordinateList();
                        _attendanceViewModel.isSubordinateDataShown = true;
                    }
                }

            }
            catch
            {
                //ignored
            }
        }

        #region DatePickers
        private void EndDate_Clicked(object sender, EventArgs e)
        {
            MaxDatePicker.Unfocus();
            MaxDatePicker.Focus();
            MaxDatePicker.Date = _attendanceViewModel.EndDate;
        }

        private void StartDate_Clicked(object sender, EventArgs e)
        {
            MinDatePicker.Unfocus();
            MinDatePicker.Focus();
            MinDatePicker.Date = _attendanceViewModel.StartDate;
        }
        #endregion

        #region Tabs Methods
        private void SelectButton(Button button, bool selected)
        {
            var darkColor = (Color)Resources["TabColor"];

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
        private async void OnTabTapped(object sender, EventArgs e)
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
                await _attendanceViewModel.SwitchMode(AttendanceMode.Attendance);
            }
            else if (button == ExceptionsButton && _attendanceViewModel.AttendanceMode != AttendanceMode.Exceptions)
            {
                _attendanceViewModel.SwitchMode(AttendanceMode.Exceptions);
            }
        }

        #endregion


        private void Sub_Tapped(object sender, HorizontaListItemTappedEventArgs e)
        {
            //Change color states of clicked day
            var employee = (Employee)e.Item;
            if (_attendanceViewModel.SelectedSub != null)
            {
                _attendanceViewModel.SelectedSub.IsSelected = false;
            }
            _attendanceViewModel.SelectedSub = employee;
            _attendanceViewModel.SelectedSub.IsSelected = true;
            if (_attendanceViewModel.AttendanceMode == AttendanceMode.Attendance)
            {
                //Load Attendance details
                _attendanceViewModel.LoadAttendanceCommand.Execute(null);
            }
            else
            {
				_attendanceViewModel.LoadExceptionsCommand.Execute(null);
            }
        }

        /// <summary>
        /// Changes color state and triggers new load of details.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Day_Tapped(object sender, HorizontaListItemTappedEventArgs e)
        {
            //Change color states of clicked day
            var newDay = (DayWrapper)e.Item;
            if (newDay.IsDummy == false)
                _attendanceViewModel.SelecteDay(newDay);
        }

        private void Date_Selected(object sender, DateChangedEventArgs e)
        {
            //_attendanceViewModel.PopulateAttendanceDays();
        }
    }
}
