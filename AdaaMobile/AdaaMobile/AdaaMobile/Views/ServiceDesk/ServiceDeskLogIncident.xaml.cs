using System;
using Xamarin.Forms;
using AdaaMobile.ViewModels;
using AdaaMobile.Strings;
using System.Collections.Generic;
using AdaaMobile.Models.Response.ServiceDesk;

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

            LoadOnBelhaf();
            LoadParentCategories();
            HandleArabicLanguageFlowDirection();
			DurationPicker.SelectedIndexChanged += DurationPicker_SelectionIndexChanged;
			AssignmentPicker.SelectedIndexChanged += AssignmentPicker_SelectionIndexChanged;
        }

		List<ParentCategory> ParentCategoryList;
		List<ChildCategory> ChildCategoryList;

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

        private async void LoadParentCategories()
        {
            lblDurationResult.Text = "Select category";
            var response = await Locator.Default.DataService.GetParentCategories(moduleName);
            loadingControl.IsRunning = false;
            if (response != null && response.ResponseStatus == AdaaMobile.DataServices.Requests.ResponseStatus.SuccessWithResult)
            {
				ParentCategoryList = response.Result.result;
                DurationPicker.Items.Clear();
                for (int i = 0; i < response.Result.result.Count; i++)
                {
					DurationPicker.Items.Add(response.Result.result[i].name);
                }
            }
        }

        private async void LoadOnBelhaf ()
        {
            lblAssignmentResult.Text = "Select name";
            var response = await Locator.Default.DataService.GetOnBelfUsers();
            loadingControl.IsRunning = false;
            if (response != null && response.ResponseStatus == AdaaMobile.DataServices.Requests.ResponseStatus.SuccessWithResult)
            {
                AssignmentPicker.Items.Clear();
                for (int i = 0; i < response.Result.result.Count; i++)
                {
					AssignmentPicker.Items.Add(response.Result.result[i].DisplayName);
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

       

		private void AssignmentPicker_SelectionIndexChanged(object sender, EventArgs e)
		{
			if (sender != null && sender is Picker)
			{
				var picker = sender as Picker;
				lblAssignmentResult.Text = AssignmentPicker.Items[picker.SelectedIndex];
			}
		}

		private async void DurationPicker_SelectionIndexChanged(object sender, EventArgs e)
		{
			if (sender != null && sender is Picker)
			{
				var picker = sender as Picker;
				var item=ParentCategoryList[picker.SelectedIndex];
				lblDurationResult.Text = item.name;
				var response = await Locator.Default.DataService.GetChildCategories(moduleName, item.id);
				if (response != null && response.ResponseStatus == AdaaMobile.DataServices.Requests.ResponseStatus.SuccessWithResult)
				{
					ChildCategoryList = response.Result.result;
				}
			}
		}

        private void ShowPassword_Toggled(object sender, ToggledEventArgs e)
        {
        }
    }
}

