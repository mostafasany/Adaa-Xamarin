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
            ReasonTypePicker.SelectedIndexChanged += ReasonTypePicker_SelectedIndexChanged;

            //Add submit action
            Action action = () =>
            {
                _dayPassViewModel.NewDayPassCommand.Execute(null);
            };
            ToolbarItems.Add(
                new ToolbarItem("", "right.png", action, ToolbarItemOrder.Primary));

            ReasonEditor.TextChanged += ReasonEditor_TextChanged;
            TextLimit.Text = string.Format("{0}/{1}", 0, 60);

			HandleArabicLanguageFlowDirection();
        }

		void HandleArabicLanguageFlowDirection()
		{
			if (Locator.Default.AppSettings.SelectedCultureName.Contains("ar"))
			{
				lblDepartureTime.HorizontalOptions = LayoutOptions.End;
				lblDepartureTimeResult.HorizontalOptions = LayoutOptions.End;
				Grid.SetColumn (imgDepartureTime, 0);
				imgDepartureTime.RotationY = 180;

				lblReturnToday.HorizontalOptions = LayoutOptions.End;
				Grid.SetColumn (lblReturnToday, 1);
				Grid.SetColumn (ReturnTodaySwitch, 0);

				lblExpectedReturnTime.HorizontalOptions = LayoutOptions.End;
				lblExpectedReturnTimeResult.HorizontalOptions = LayoutOptions.End;
				Grid.SetColumn (imgExpectedReturnTime, 0);
				imgExpectedReturnTime.RotationY = 180;

				lblDuration.HorizontalOptions = LayoutOptions.End;
				lblDurationResult.HorizontalOptions = LayoutOptions.End;

				lblReasonType.HorizontalOptions = LayoutOptions.End;
				lblReasonTypeResult.HorizontalOptions = LayoutOptions.End;
				Grid.SetColumn (imgReasonType, 0);
				imgReasonType.RotationY = 180;

				lblReason.HorizontalOptions = LayoutOptions.End;


				Grid.SetColumn (lblTextLimitText, 1);
				lblTextLimitText.HorizontalOptions = LayoutOptions.End;
				Grid.SetColumn (TextLimit, 0);
				TextLimit.HorizontalOptions = LayoutOptions.Start;
			
			}
		}

        private void EndTime_OnClicked(object sender, EventArgs e)
        {
            EndTimePicker.Unfocus();
            EndTimePicker.Focus();
        }


        private void StartTime_OnClicked(object sender, EventArgs e)
        {
            StartTimePicker.Unfocus();
            StartTimePicker.Focus();
        }

        private void ReasonType_OnClicked(object sender, EventArgs e)
        {
            ReasonTypePicker.Unfocus();
            ReasonTypePicker.Focus();
        }

        private void ReasonEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewTextValue))
            {
                TextLimit.Text = string.Format("{0}/{1}", e.NewTextValue.Length, 60);
            }
        }

        private void EndTime_Tapped(object sender, EventArgs e)
        {
            EndTimePicker.Unfocus();
            EndTimePicker.Focus();
        }

        private void StartTime_Tapped(object sender, EventArgs e)
        {
            StartTimePicker.Unfocus();
            StartTimePicker.Focus();
        }

        private void ReasonType_Tapped(object sender, EventArgs e)
        {
            ReasonTypePicker.Unfocus();
            ReasonTypePicker.Focus();
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
    }
}
