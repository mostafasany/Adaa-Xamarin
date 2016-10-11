using System;
using System.Collections.Generic;
using Xamarin.Forms;
using AdaaMobile.ViewModels;
using AdaaMobile.Strings;
using AdaaMobile.Models.Response;
using AdaaMobile.Models.Request;

namespace AdaaMobile
{
    public partial class AddTask : ContentPage
    {
        private readonly MyTimeSheetViewModel _viewModel;
        List<Assignment> assigmnets;
        List<AttendanceTask> tasks;

        public AddTask()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");

            _viewModel = Locator.Default.MyTimeSheetViewModel;
            BindingContext = _viewModel;

            Title = AppResources.TimeSheet_AddNewTask;

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
            HandleArabicLanguageFlowDirection();
        }

        void HandleArabicLanguageFlowDirection()
        {
            if (Locator.Default.AppSettings.SelectedCultureName.Contains("ar"))
            {
                lblDuration.HorizontalOptions = LayoutOptions.End;
                lblDurationResult.HorizontalOptions = LayoutOptions.End;
                Grid.SetColumn(imageReasonType, 0);
                imageReasonType.RotationY = 180;

                lblAssignment.HorizontalOptions = LayoutOptions.End;
                lblAssignmentResult.HorizontalOptions = LayoutOptions.End;
                Grid.SetColumn(imageAssignmentType, 0);
                imageAssignmentType.RotationY = 180;

                lblTask.HorizontalOptions = LayoutOptions.End;
                lblTaskResult.HorizontalOptions = LayoutOptions.End;
                Grid.SetColumn(imageTask, 0);
                imageTask.RotationY = 180;

                lblAdditionalComments.HorizontalOptions = LayoutOptions.End;

            }
        }

        private async void AddNewTask()
        {
            try
            {
                loadingControl.IsRunning = true;
                int assignId, taskId;
                string duration, comment;
                if (AssignmentPicker.SelectedIndex > -1)
                {
                    assignId = assigmnets[AssignmentPicker.SelectedIndex].Id;

                }
                else
                {
                    await Locator.Default.DialogManager.DisplayAlert(AppResources.ApplicationName, AppResources.TimeSheet_EnterAssigmnet, AppResources.Ok);

                    return;
                }
                if (TaskPicker.SelectedIndex > -1)
                {
                    taskId = tasks[TaskPicker.SelectedIndex].ID;

                }
                else
                {
                    await Locator.Default.DialogManager.DisplayAlert(AppResources.ApplicationName, AppResources.TimeSheet_EnterTask, AppResources.Ok);
                    return;
                }

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

                string day = _viewModel.SelectedDay.Date.ToString("dddd");
                TimeSheetRequest timeSheetDetails = new TimeSheetRequest();
                List<TimeSheetRequest> timeSheetList = new List<TimeSheetRequest>();
                timeSheetDetails.AssignmentID = assignId;
                timeSheetDetails.TaskID = taskId;



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

                if(double.Parse(duration) > _viewModel.TimeSheetFormated.LoggedInHours)
                {
                    await Locator.Default.DialogManager.DisplayAlert(AppResources.ApplicationName, AppResources.TimeSheet_Exceed8Hours, AppResources.Ok);
                    return;
                }

                timeSheetList.Add(timeSheetDetails);
                var status = await Locator.Default.DataService.SubmitTimeSheet(DateTime.Now.Year, _viewModel.SelectedWeek.WeekNumber, timeSheetList, null);
                if (status.Result)
                {
                    loadingControl.IsRunning = false;
                    _viewModel.IsRefreshRequired = true;
                    Locator.Default.NavigationService.GoBack();
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

        private async void LoadAssigments()
        {
            lblAssignmentResult.Text = AppResources.TimeSheet_SelectAssigmnet;
            lblTaskResult.Text = AppResources.TimeSheet_SelectTask;
            lblDurationResult.Text = AppResources.TimeSheet_SelectDuration;
            var response = await Locator.Default.DataService.GetAssignmentAsync();
            loadingControl.IsRunning = false;
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
            for (int i = 0; i < 9; i++)
            {
                string value = i.ToString() + ":00 H";
                DurationPicker.Items.Add(value);
                if(i!=8)
                {
                    value = i.ToString() + ":30 H";
                    DurationPicker.Items.Add(value);
                }
              
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
            try
            {
                loadingControl.IsRunning = true;

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
                            TaskPicker.SelectedIndex = 0;
                        }
                    }
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

        private async void TaskPicker_SelectionIndexChanged(object sender, EventArgs e)
        {
            if (sender != null && sender is Picker)
            {
                var picker = sender as Picker;
                if (picker.SelectedIndex >= 0)
                {
                    lblTaskResult.Text = tasks[picker.SelectedIndex].Name;
                }

            }
        }
    }
}

