using System;
using Xamarin.Forms;
using AdaaMobile.ViewModels;
using AdaaMobile.Strings;

namespace AdaaMobile
{
    public partial class ServiceDeskLogIncident : ContentPage
    {
        private readonly ServiceDeskLogIncidentViewModel _viewModel;
        string moduleName = "Incident%20Classification";
        public ServiceDeskLogIncident()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");

            _viewModel = Locator.Default.ServiceDeskLogIncidentViewModel;
            BindingContext = _viewModel;

            Title = AppResources.TimeSheet_AddNewTask;

            //Add submit action
            Action action = () =>
            {
                LogIncident();
            };
            ToolbarItems.Add(
                new ToolbarItem("", "right.png", action, ToolbarItemOrder.Primary));

            LoadOnBelhaf();
            LoadParentCategories();
            HandleArabicLanguageFlowDirection();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.PageLoadedCommand.Execute(null);
        }

        void HandleArabicLanguageFlowDirection()
        {
            if (Locator.Default.AppSettings.SelectedCultureName.Contains("ar"))
            {
                lblDuration.HorizontalOptions = LayoutOptions.End;
                lblDurationResult.HorizontalOptions = LayoutOptions.End;
                Grid.SetColumn(imageReasonType, 0);
                imageReasonType.RotationY = 180;

            

            }
        }

        private async void LoadOnBelhaf()
        {
            lblDurationResult.Text = "Select name";
            var response = await Locator.Default.DataService.GetOnBelfUsers();
            loadingControl.IsRunning = false;
            if (response != null && response.ResponseStatus == AdaaMobile.DataServices.Requests.ResponseStatus.SuccessWithResult)
            {
                DurationPicker.Items.Clear();
                for (int i = 0; i < response.Result.result.Count; i++)
                {
                    DurationPicker.Items.Add(response.Result.result[i].ToString());
                }
            }
        }

        private async void LoadParentCategories()
        {
            lblAssignmentResult.Text = "Select categories";
            var response = await Locator.Default.DataService.GetParentCategories(moduleName);
            loadingControl.IsRunning = false;
            if (response != null && response.ResponseStatus == AdaaMobile.DataServices.Requests.ResponseStatus.SuccessWithResult)
            {
                AssignmentPicker.Items.Clear();
                for (int i = 0; i < response.Result.result.Count; i++)
                {
                    AssignmentPicker.Items.Add(response.Result.result[i].ToString());
                }
            }
        }

        private async void LogIncident()
        {
        }

		private void Assignment_OnClicked(object sender, EventArgs e)
		{
			AssignmentPicker.Unfocus();
			AssignmentPicker.Focus();
		}

        private void Duration_OnClicked(object sender, EventArgs e)
        {
            DurationPicker.Unfocus();
            DurationPicker.Focus();
        }

        private void DurationPicker_SelectionIndexChanged(object sender, EventArgs e)
        {
            if (sender != null && sender is Picker)
            {
                var picker = sender as Picker;
                lblDurationResult.Text = DurationPicker.Items[picker.SelectedIndex];
            }
        }

        private void ShowPassword_Toggled(object sender, ToggledEventArgs e)
        {
        }
    }
}

