using System;
using Xamarin.Forms;
using AdaaMobile.ViewModels;
using AdaaMobile.Strings;
using AdaaMobile.Models.Request;
using System.Collections.Generic;
using AdaaMobile.DataServices;

namespace AdaaMobile
{

    public partial class EditTask : ContentPage
    {
        private readonly MyTimeSheetViewModel _viewModel;
		private readonly IDataService _dataService;

		public EditTask()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");

            _viewModel = Locator.Default.MyTimeSheetViewModel;
            BindingContext = _viewModel;

            Title = AppResources.EServices;

            //Add submit action
            Action action = () =>
            {
                EditSelectedTask();
            };
            ToolbarItems.Add(
                new ToolbarItem("", "right.png", action, ToolbarItemOrder.Primary));

            LoadDuration();
            DurationPicker.SelectedIndexChanged += DurationPicker_SelectionIndexChanged;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.PageLoadedCommand.Execute(null);
        }


        private async void EditSelectedTask()
        {
            string duration, comment;
			int taskId, assignId;
			TimeSheetListRequest timeSheetRequest = new TimeSheetListRequest();
			TimeSheetRequest timeSheetDetails = new TimeSheetRequest();
			List<TimeSheetRequest> timeSheetList = new List<TimeSheetRequest>();
			string day = _viewModel.SelectedProjectTask.Day.DayName;


			if (!string.IsNullOrEmpty(lblDurationResult.Text))
            {
                duration = lblDurationResult.Text;
            }
            if (!string.IsNullOrEmpty(AdditionalCommentsEditor.Text))
            {
                comment = AdditionalCommentsEditor.Text;
            }
            if (_viewModel.SelectedProjectTask != null)
            {
				taskId = Convert.ToInt32(_viewModel.SelectedProjectTask.Id);
				assignId = Convert.ToInt32(_viewModel.SelectedProjectTask.AssigmentId);

				if (day == "Sunday")
				{
					timeSheetDetails.Sunday = duration;
					timeSheetDetails.SundayComment = comment;
				}
				else if (day == "Monday")
				{
					timeSheetDetails.Monday = duration;
					timeSheetDetails.MondayComment = comment;

				}
				else if (day == "Tuesday")
				{
					timeSheetDetails.Tuesday = duration;
					timeSheetDetails.TuesdayComment = comment;
				}
				else if (day == "Wednesday")
				{
					timeSheetDetails.Wednesday = duration;
					timeSheetDetails.WednesdayComment = comment;
				}
				else if (day == "Thursday")
				{
					timeSheetDetails.Thursday = duration;
					timeSheetDetails.ThursdayComment = comment;
				}
				timeSheetDetails.AssignmentID = assignId;
				timeSheetDetails.TaskID = taskId;
			}

			timeSheetList.Add(timeSheetDetails);
			await	_dataService.SubmitTimeSheet(DateTime.Now.Year, Convert.ToInt32(_viewModel.SelectedWeek.WeekText), "", timeSheetRequest, null);

        }


        private void LoadDuration()
        {
            DurationPicker.Items.Clear();
            lblDurationResult.Text = "Select Duration";
            for (int i = 0; i < 24; i++)
            {
                string value = i.ToString() + ":00";
                DurationPicker.Items.Add(value);
                value = i.ToString() + ":30";
                DurationPicker.Items.Add(value);
            }
            DurationPicker.SelectedIndexChanged += DurationPicker_SelectionIndexChanged;
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
    }
}

