using System;
using AdaaMobile.ViewModels;
using Xamarin.Forms;
using AdaaMobile.Helpers;
using AdaaMobile.Strings;

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
            Title = AppResources.NewRequest;
            StartTimeBtn.Clicked += StartTimeBtn_Clicked;
            EndTimeBtn.Clicked += EndTimeBtn_Clicked;
            ReasonEditor.Behaviors.Add(new MaxLengthValidator() { MaxLength = 60 });
            ReasonEditor.TextChanged += ReasonEditor_TextChanged;
        }

        private void ReasonEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewTextValue))
            {
                TextLimit.Text = string.Format("{0}/{1}", e.NewTextValue.Length, 60);
            }
        }

        void EndTimeBtn_Clicked(object sender, EventArgs e)
        {
            EndTimePicker.Unfocus();
            EndTimePicker.Focus();
        }

        void StartTimeBtn_Clicked(object sender, EventArgs e)
        {
            StartTimePicker.Unfocus();
            StartTimePicker.Focus();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            NavigationPage.SetBackButtonTitle(this, string.Empty);
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
