using System;
using AdaaMobile.ViewModels;
using Xamarin.Forms;
using AdaaMobile.Helpers;
using AdaaMobile.Strings;

namespace AdaaMobile.Views.DayPass
{
    public partial class NewDayPassRequestPage : ContentPage
    {

        private readonly NewDayPassViewModel _dayPassViewModel;
        public NewDayPassRequestPage()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");
            Title = AppResources.NewRequest;

            //Assign data context
            _dayPassViewModel = Locator.Default.NewDayPassViewModel;
            BindingContext = _dayPassViewModel;

            //Initialize picker and wire events
            ReasonTypePicker.Items.Add(AppResources.Work);
            ReasonTypePicker.Items.Add(AppResources.Personal);
            ReasonTypeButton.Clicked += ReasonTypeButton_Clicked;
            ReasonTypePicker.SelectedIndexChanged += ReasonTypePicker_SelectedIndexChanged;

            //Add submit action
            Action action = () =>
            {
                _dayPassViewModel.NewDayPassCommand.Execute(null);
            };
            ToolbarItems.Add(
                new ToolbarItem("", "right.png", action, ToolbarItemOrder.Primary));
            

            StartTimeBtn.Clicked += StartTimeBtn_Clicked;
            EndTimeBtn.Clicked += EndTimeBtn_Clicked;

            //ReasonEditor.Behaviors.Add(new MaxLengthValidator() { MaxLength = 60 });
            ReasonEditor.TextChanged += ReasonEditor_TextChanged;
            TextLimit.Text = string.Format("{0}/{1}", 0, 60);
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
            if (ReasonTypePicker.SelectedIndex == 0)
            {
                _dayPassViewModel.ReasonType = "Work";
                _dayPassViewModel.LocalizedReasonType = AppResources.Work;
            }
            else
            {
                _dayPassViewModel.ReasonType = "Personal";
                _dayPassViewModel.LocalizedReasonType = AppResources.Personal;
            }
        }


        private void ReasonTypeButton_Clicked(object sender, EventArgs e)
        {
            ReasonTypePicker.Unfocus();
            ReasonTypePicker.Focus();
        }


    }
}
