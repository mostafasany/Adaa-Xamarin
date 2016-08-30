using System;
using Xamarin.Forms;
using AdaaMobile.ViewModels;
using AdaaMobile.Strings;
using AdaaMobile.Models.Request;
using System.Collections.Generic;

namespace AdaaMobile
{
	public partial class EditTask : ContentPage
	{
		private readonly MyTimeSheetViewModel _viewModel;

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
				lblAdditionalComments.HorizontalOptions = LayoutOptions.End;

			}
		}

		private async void EditSelectedTask()
		{
			try
			{


				loadingControl.IsRunning = true;
				string duration = "", comment = "";
				int taskId, assignId;
				TimeSheetRequest timeSheetDetails = new TimeSheetRequest();
				List<TimeSheetRequest> timeSheetList = new List<TimeSheetRequest>();
				string day = _viewModel.SelectedProjectTask.Day.DayName;


				if (DurationPicker.SelectedIndex > -1)
				{
					duration = lblDurationResult.Text.Replace('H', ' ').Replace(':', '.').Trim();
				}
				else
				{
					await Locator.Default.DialogManager.DisplayAlert(AppResources.ApplicationName, AppResources.TimeSheet_EnterDuration, AppResources.Ok);
					return;

				}
				if (!string.IsNullOrEmpty(AdditionalCommentsEditor.Text))
				{
					comment = AdditionalCommentsEditor.Text;
				}
				else
				{
					await Locator.Default.DialogManager.DisplayAlert(AppResources.ApplicationName, AppResources.TimeSheet_EnterComment, AppResources.Ok);
					return;

				}
				if (_viewModel.SelectedProjectTask != null)
				{
					taskId = Convert.ToInt32(_viewModel.SelectedProjectTask.Id);
					assignId = Convert.ToInt32(_viewModel.SelectedProjectTask.AssigmentId);

					if (day == AppResources.Sunday)
					{
						timeSheetDetails.Sunday = duration;
						timeSheetDetails.SundayComment = comment;
					}
					else if (day == AppResources.Monday)
					{
						timeSheetDetails.Monday = duration;
						timeSheetDetails.MondayComment = comment;

					}
					else if (day == AppResources.Tuesday)
					{
						timeSheetDetails.Tuesday = duration;
						timeSheetDetails.TuesdayComment = comment;
					}
					else if (day == AppResources.Wednesday)
					{
						timeSheetDetails.Wednesday = duration;
						timeSheetDetails.WednesdayComment = comment;
					}
					else if (day == AppResources.Thursday)
					{
						timeSheetDetails.Thursday = duration;
						timeSheetDetails.ThursdayComment = comment;
					}
					timeSheetDetails.AssignmentID = assignId;
					timeSheetDetails.TaskID = taskId;
				}

				timeSheetList.Add(timeSheetDetails);
				var status = await Locator.Default.DataService.SubmitTimeSheet(DateTime.Now.Year, _viewModel.SelectedWeek.WeekNumber, timeSheetList, null);
				if (status.Result)
				{
					loadingControl.IsRunning = false;
					_viewModel.IsRefreshRequired = true;
					//Locator.Default.NavigationService.GoBack();
					//Locator.Default.NavigationService..GoBack();
					Locator.Default.NavigationService.NavigateToPage(typeof(MyTimesheetPage));
				}
				else
				{
					loadingControl.IsRunning = false;
					await Locator.Default.DialogManager.DisplayAlert(AppResources.ApplicationName, AppResources.TimeSheet_ErrorHappened, AppResources.Ok);
				}
			}
			catch (Exception ex)
			{

			}
			finally
			{
				loadingControl.IsRunning = false;
			}
		}

		private void LoadDuration()
		{
			DurationPicker.Items.Clear();
			lblDurationResult.Text = AppResources.TimeSheet_SelectDuration;
			for (int i = 0; i < 24; i++)
			{
				string value = i.ToString() + ":00 H";
				DurationPicker.Items.Add(value);
				value = i.ToString() + ":30 H";
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

