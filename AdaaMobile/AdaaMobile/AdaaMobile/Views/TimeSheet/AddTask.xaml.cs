using System;
using System.Collections.Generic;

using Xamarin.Forms;
using AdaaMobile.ViewModels;
using AdaaMobile.Strings;
using AdaaMobile.Views.TimeSheet;
using AdaaMobile.Models.Response;
using AdaaMobile.DataServices;
using AdaaMobile.Models.Request;

namespace AdaaMobile
{

	public partial class AddTask : ContentPage
	{
		private readonly IDataService _dataService;
		private readonly MyTimeSheetViewModel _viewModel;
		List<Assignment> assigmnets;
		List<AttendanceTask> tasks;

		public AddTask()
		{
			InitializeComponent();
			NavigationPage.SetBackButtonTitle(this, "");

			_viewModel = Locator.Default.MyTimeSheetViewModel;
			BindingContext = _viewModel;

			Title = AppResources.EServices;

			//Add submit action
			Action action = () =>
			{
				AddNewTask();
			};
			ToolbarItems.Add(
				new ToolbarItem("", "right.png", action, ToolbarItemOrder.Primary));


			LoadDuration();
			LoadAssigments();
			DurationPicker.SelectedIndexChanged += DurationPicker_SelectionIndexChanged;
			AssignmentPicker.SelectedIndexChanged += AssignmentPicker_SelectionIndexChanged;
			TaskPicker.SelectedIndexChanged += TaskPicker_SelectionIndexChanged;
		}


		private void AddNewTask()
		{
			int assignId = assigmnets[AssignmentPicker.SelectedIndex].Id;
			int taskId = tasks[TaskPicker.SelectedIndex].ID;
			string duration = lblDurationResult.Text;
			string comment = AdditionalCommentsEditor.Text;
			string day = _viewModel.SelectedDay.Date.ToString("dddd");
			TimeSheetListRequest timeSheetRequest = new TimeSheetListRequest();
			TimeSheetRequest timeSheetDetails = new TimeSheetRequest();
			List<TimeSheetRequest> timeSheetList = new List<TimeSheetRequest>();

			timeSheetDetails.AssignmentID = assignId;
			timeSheetDetails.TaskID = taskId;
			//timeSheetDetails.TaskID = Convert.ToInt32(_viewModel.SelectedProjectTask.Id);
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

			timeSheetList.Add(timeSheetDetails);
			_dataService.SubmitTimeSheet(DateTime.Now.Year, Convert.ToInt32(_viewModel.SelectedWeek.WeekText), "", timeSheetRequest, null);
		}
		private async void LoadAssigments()
		{
			lblAssignmentResult.Text = "Select Assignment";
			lblTaskResult.Text = "Select Task";
			lblDurationResult.Text = "Select Duration";
			var response = await Locator.Default.DataService.GetAssignmentAsync();

			if (response != null && response.ResponseStatus == AdaaMobile.DataServices.Requests.ResponseStatus.SuccessWithResult)
			{
				AssignmentPicker.Items.Clear();
				assigmnets = response.Result;
				for (int i = 0; i < assigmnets.Count; i++)
				{
					AssignmentPicker.Items.Add(assigmnets[i].ToString());
				}
			}

		}

		private void LoadDuration()
		{
			DurationPicker.Items.Clear();
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

		private void Assignment_OnClicked(object sender, EventArgs e)
		{
			AssignmentPicker.Unfocus();
			AssignmentPicker.Focus();
		}


		private void SelectTask_OnClicked(object sender, EventArgs e)
		{
			TaskPicker.Unfocus();
			TaskPicker.Focus();
		}

		private void DurationPicker_SelectionIndexChanged(object sender, EventArgs e)
		{
			if (sender != null && sender is Picker)
			{
				var picker = sender as Picker;
				lblDurationResult.Text = DurationPicker.Items[picker.SelectedIndex];
			}
		}

		private async void AssignmentPicker_SelectionIndexChanged(object sender, EventArgs e)
		{
			if (sender != null && sender is Picker)
			{

				var picker = sender as Picker;
				lblAssignmentResult.Text = assigmnets[picker.SelectedIndex].Title;

				var response = await Locator.Default.DataService.GetTaskByAssignment(assigmnets[picker.SelectedIndex].Id, null);

				if (response != null && response.ResponseStatus == AdaaMobile.DataServices.Requests.ResponseStatus.SuccessWithResult)
				{
					TaskPicker.Items.Clear();
					tasks = response.Result;
					lblTaskResult.Text = tasks[0].Name;
					for (int i = 0; i < tasks.Count; i++)
					{
						TaskPicker.Items.Add(tasks[i].Name);
					}
				}
			}
		}

		private async void TaskPicker_SelectionIndexChanged(object sender, EventArgs e)
		{
			if (sender != null && sender is Picker)
			{
				var picker = sender as Picker;
				lblTaskResult.Text = tasks[0].Name;

			}
		}
	}
}

