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
			Title = "New Request";
			StartTimeBtn.Clicked += StartTimeBtn_Clicked;
			EndTimeBtn.Clicked += EndTimeBtn_Clicked;
        }



        void EndTimeBtn_Clicked (object sender, EventArgs e)
        {
			EndTimePicker.Unfocus ();
			EndTimePicker.Focus ();
        }

        void StartTimeBtn_Clicked (object sender, EventArgs e)
        {
			StartTimePicker.Unfocus ();
			StartTimePicker.Focus ();
        }

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
			NavigationPage.SetBackButtonTitle (this, string.Empty);
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
