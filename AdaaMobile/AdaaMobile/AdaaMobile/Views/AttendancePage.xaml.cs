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
        public AttendancePage()
        {
            InitializeComponent();
            _attendanceViewModel = Locator.Default.AttendanceViewModel;
            BindingContext = _attendanceViewModel;
            PopuplateDaysStack();
            Action action = () => { };
            ToolbarItems.Add(
                new ToolbarItem("Refresh", "icon.png", action, ToolbarItemOrder.Default));
        }

        private void PopuplateDaysStack()
        {
            DaysStack.BatchBegin();
            DaysStack.Children.Clear();
            foreach (var day in _attendanceViewModel.DaysList)
            {
                var label = new Label()
                {
                    Text = day,
                    TextColor = Color.Black,
                    BackgroundColor = Color.White,
                    WidthRequest = 60,
                    HeightRequest = 60,
                };
                DaysStack.Children.Add(label);
            }
            DaysStack.BatchCommit();
            DaysScroll.ForceLayout();
        }
    }
}
