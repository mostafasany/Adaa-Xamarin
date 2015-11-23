using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private Layout _lastTappedGrid;
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
            Action action = () => { };
            ToolbarItems.Add(
                new ToolbarItem("Refresh", "icon.png", action, ToolbarItemOrder.Default));

            //Select Tabs
            SelectButton(AttendanceButton, true);
            SelectButton(ExceptionsButton, false);
            _lastTappedTab = AttendanceButton;

        }

        #region Days List Related Methods
        private void Day_Tapped(object sender, EventArgs e)
        {
            var tappedGrid = (Layout)sender;
            if (tappedGrid == _lastTappedGrid) return;
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

        private void SetTextColor(Layout grid, string labelName, Color color)
        {
            var label = grid.FindByName<Label>(labelName);
            if (label != null)
            {
                label.TextColor = color;
            }
        }

        private void SetBackColor(Layout grid, string boxName, Color color)
        {
            var boxView = grid.FindByName<BoxView>(boxName);
            if (boxView != null)
            {
                boxView.BackgroundColor = color;
            }
        }
        #endregion

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
            var darkColor = (Color)Application.Current.Resources["AppBackgroundDark"];

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
            SelectButton(button, true);
            if (_lastTappedTab != null)
            {
                SelectButton(_lastTappedTab, false);
            }
            _lastTappedTab = button;
        }

        #endregion

        private void DayItem_Tapped(object sender, ItemTappedEventArgs e)
        {
            Debug.WriteLine("ItemTapped");
            var newDay = (DayWrapper)e.Item;
            newDay.IsSelected = true;
            var oldDay = _attendanceViewModel.SelectedDay;
            if (oldDay != null && oldDay != newDay)
                oldDay.IsSelected = false;
            _attendanceViewModel.SelectedDay = newDay;
        }
    }
}
