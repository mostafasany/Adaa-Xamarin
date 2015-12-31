using System;
using AdaaMobile.ViewModels;
using Xamarin.Forms;

namespace AdaaMobile.Views.DayPass
{
    public partial class NewDayPassRequestPage : ContentPage
    {

        private DayPassViewModel _dayPassViewModel;
        public NewDayPassRequestPage()
        {
            InitializeComponent();
            _dayPassViewModel = ViewModels.Locator.Default.DayPassViewModel;
            BindingContext = _dayPassViewModel;
            ReasonTypeButton.Clicked += ReasonTypeButton_Clicked;
            ReasonTypePicker.SelectedIndexChanged += ReasonTypePicker_SelectedIndexChanged;
            Action action = () =>
            {
                _dayPassViewModel.NewDayPassCommand.Execute(null);
            };
            ToolbarItems.Add(
                new ToolbarItem("", "right.png", action, ToolbarItemOrder.Primary));

        }

        private void ReasonTypePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            _dayPassViewModel.ReasonType = ReasonTypePicker.SelectedIndex == 0 ? "Work" : "Personal";
        }

        private void ReasonTypePicker_Unfocused(object sender, FocusEventArgs e)
        {

        }

        private void ReasonTypeButton_Clicked(object sender, EventArgs e)
        {
            ReasonTypePicker.Unfocus();
            ReasonTypePicker.Focus();
        }


    }
}
