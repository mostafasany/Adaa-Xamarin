using AdaaMobile.DataServices;
using AdaaMobile.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdaaMobile.Common;
using AdaaMobile.Models;
using AdaaMobile.Models.Response;
using System.Collections.ObjectModel;
using AdaaMobile.DataServices.Requests;
using System.Globalization;
using AdaaMobile.Strings;

namespace AdaaMobile.ViewModels
{
	public class MyTimeSheetViewModel : BindableBase
	{
		#region Fields

		private readonly INavigationService _navigationService;
		private readonly IDataService _dataService;
		private readonly int SmallestWindowLimit;
		private ResponseWrapper<TimeSheet> timeSheetResponse;
		#endregion

		#region Properties

		private bool _IsBusy;
		public bool IsBusy
		{
			get { return _IsBusy; }
			set
			{
				SetProperty(ref _IsBusy, value);
			}
		}

		private List<DayWrapper> _daysList;
		public List<DayWrapper> DaysList
		{
			get { return _daysList; }
			set { SetProperty(ref _daysList, value); }
		}

		private List<Week> _WeekList;
		public List<Week> WeekList
		{
			get { return _WeekList; }
			set { SetProperty(ref _WeekList, value); }
		}

		private Week _SelectedWeek;
		public Week SelectedWeek
		{
			get { return _SelectedWeek; }
			set { SetProperty(ref _SelectedWeek, value); }
		}

		private DayWrapper _selectedDay;
		public DayWrapper SelectedDay
		{
			get { return _selectedDay; }
			set { SetProperty(ref _selectedDay, value); }
		}

		private TimeSheetFormated _TimeSheetFormated;
		public TimeSheetFormated TimeSheetFormated
		{
			get { return _TimeSheetFormated; }
			set { SetProperty(ref _TimeSheetFormated, value); }
		}

		private ProjectTask _SelectedProjectTask;
		public ProjectTask SelectedProjectTask
		{
			get { return _SelectedProjectTask; }
			set { SetProperty(ref _SelectedProjectTask, value); }
		}

		private ObservableCollection<Grouping<Project, ProjectTask>> _GroupedTimeSheet;
		public ObservableCollection<Grouping<Project, ProjectTask>> GroupedTimeSheet
		{
			get { return _GroupedTimeSheet; }
			set { SetProperty(ref _GroupedTimeSheet, value); }
		}

		private bool _NoProjectsExists;
		public bool NoProjectsExists
		{
			get { return _NoProjectsExists; }
			set { SetProperty(ref _NoProjectsExists, value); }
		}

		private bool _ProjectsExists;
		public bool ProjectsExists
		{
			get { return _ProjectsExists; }
			set { SetProperty(ref _ProjectsExists, value); }
		}

		private bool _IsAddTaskButtonVisible;
		public bool IsAddTaskButtonVisible
		{
			get { return _IsAddTaskButtonVisible; }
			set { SetProperty(ref _IsAddTaskButtonVisible, value); }
		}

		#endregion

		#region Initialization

		public MyTimeSheetViewModel(INavigationService navigationService, IDataService dataservice)
		{
			_navigationService = navigationService;
			_dataService = dataservice;
			PageLoadedCommand = new AsyncExtendedCommand(Loaded);
			AddNewTaskCommand = new AsyncExtendedCommand(AddNewTask);
			RequestItemSelectedCommand = new AsyncExtendedCommand<ProjectTask>(OpenRequestDetailsPage);
			SmallestWindowLimit = Xamarin.Forms.Device.OnPlatform(5, 14, 14);
		}

		#endregion

		#region Commands
		public AsyncExtendedCommand PageLoadedCommand { get; set; }
		public AsyncExtendedCommand AddNewTaskCommand { get; set; }
		public AsyncExtendedCommand<ProjectTask> RequestItemSelectedCommand { get; set; }
		public bool IsRefreshRequired { get; internal set; } = true;
		#endregion

		#region Methods

		private async Task Loaded()
		{
			try
			{
				if (WeekList != null && WeekList.Count > 0 && !IsRefreshRequired)
					return;
				GroupedTimeSheet = new ObservableCollection<Grouping<Project, ProjectTask>>();
				TimeSheetFormated = new TimeSheetFormated();
				NoProjectsExists = true;
				ProjectsExists = false;

				IsBusy = true;

				var response = await _dataService.GetWeeksPerYearAsync(DateTime.Now.Year, null);
				if (response != null && response.ResponseStatus == DataServices.Requests.ResponseStatus.SuccessWithResult)
				{
					var currentCulture = CultureInfo.CurrentCulture;
					var weekNo = currentCulture.Calendar.GetWeekOfYear(
						DateTime.Now,
									currentCulture.DateTimeFormat.CalendarWeekRule,
									currentCulture.DateTimeFormat.FirstDayOfWeek);

					WeekList = response.Result;
					SelectWeek(weekNo - 2);
				}
			}
			catch (Exception ex)
			{
			}
			finally
			{
				IsBusy = false;
			}

		}

		private async Task OpenRequestDetailsPage(ProjectTask projectTask)
		{
			if (projectTask is ProjectTask)
			{
				SelectedProjectTask = projectTask as ProjectTask;
				_navigationService.NavigateToPage(typeof(TaskDetails));
			}

		}

		private async Task AddNewTask()
		{
			_navigationService.NavigateToPage(typeof(AddTask));
		}

		#region WeekDays

		private void FixWindowSize(List<DayWrapper> daysList)
		{
			if (daysList.Count < SmallestWindowLimit)
			{
				for (int i = daysList.Count; i < SmallestWindowLimit; i++)
				{
					daysList.Add(new DayWrapper(new DateTime(), true));
				}
			}
		}

		async void SelectWeek(int weekIndex)
		{
			try
			{
				GroupedTimeSheet = new ObservableCollection<Grouping<Project, ProjectTask>>();
				TimeSheetFormated = new TimeSheetFormated();
				NoProjectsExists = true;
				ProjectsExists = false;

				IsBusy = true;

				SelectedWeek = WeekList[weekIndex];
				List<DayWrapper> daysList = await Task.Run(() =>
				{
					var days = new List<DayWrapper>();
					var currentDate = SelectedWeek.WeekStart;
					var endDate = SelectedWeek.WeekEnd;
					while (currentDate <= endDate)
					{
						if (currentDate.DayOfWeek != DayOfWeek.Friday && currentDate.DayOfWeek != DayOfWeek.Saturday)
							days.Add(new DayWrapper(currentDate, false));
						currentDate = currentDate.AddDays(1);
					}
					return days;
				});

				if (Locator.Default.AppSettings.SelectedCultureName.Contains("ar"))
				{
					daysList.Reverse();
				}

				FixWindowSize(daysList);

				DaysList = daysList;
				var nowDay = daysList.FirstOrDefault(a => a.Date.Day == DateTime.Now.Day);
				if (nowDay == null)
				{
					nowDay = daysList.FirstOrDefault();
				}
				SelecteDay(nowDay, true);
			}
			catch (Exception ex)
			{
				IsBusy = false;
			}
			finally
			{

			}


		}

		public void GetNextWeek()
		{
			var indexOfCurrentWeek = WeekList.IndexOf(SelectedWeek);
			var nextWeek = ++indexOfCurrentWeek;
			if (nextWeek < WeekList.Count)
			{
				SelectWeek(nextWeek);
			}
		}

		public void GetPreviousWeek()
		{
			var indexOfCurrentWeek = WeekList.IndexOf(SelectedWeek);
			var prevWeek = --indexOfCurrentWeek;
			if (prevWeek >= 0)
			{
				SelectWeek(prevWeek);
			}
		}

		public async void SelecteDay(DayWrapper day, bool isWeekChanged)
		{
			try
			{
				if (!IsRefreshRequired && SelectedDay != null && day.Date == SelectedDay.Date)
				{
					return;
				}
				GroupedTimeSheet = new ObservableCollection<Grouping<Project, ProjectTask>>();
				TimeSheetFormated = new TimeSheetFormated();
				NoProjectsExists = true;
				ProjectsExists = false;
				IsAddTaskButtonVisible = isBetween(day.Date);
				IsBusy = true;
				if (day == null)
					return;
				if (day.IsDummy)
					return;
				if (day == SelectedDay)
					return;

				//Unselect previous day
				if (SelectedDay != null)
					SelectedDay.IsSelected = false;

				//Select day

				day.IsSelected = true;
				SelectedDay = day;

				if (SelectedDay != null)
					await LoadTimeSheet(SelectedWeek.WeekNumber, DateTime.Now.Year, SelectedDay.Date, isWeekChanged);

			}
			catch (Exception ex)
			{
			}
			finally
			{
				IsBusy = false;
			}
		}

		public bool isBetween(DateTime input)
		{
			var date1 = FirstDayOfWeek(DateTime.Now);
			var date2 = LastDayOfWeek(DateTime.Now);
			if (input.Date >= date1.Date && input.Date <= date2.Date)
				return true;
			else
				return false;
		}

		public static DateTime FirstDayOfWeek(DateTime date)
		{
			DayOfWeek fdow = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
			int offset = fdow - date.DayOfWeek;
			DateTime fdowDate = date.AddDays(offset);
			return fdowDate;
		}

		public static DateTime LastDayOfWeek(DateTime date)
		{
			DateTime ldowDate = FirstDayOfWeek(date).AddDays(6);
			return ldowDate;
		}

		#endregion

		#region TimeSheet

		public async Task LoadTimeSheet(int weekNo, int year, DateTime selectedDay, bool isWeekChanged)
		{
			try
			{
				IsBusy = true;
				if (IsRefreshRequired || isWeekChanged || timeSheetResponse == null)
				{
					timeSheetResponse = await _dataService.GetTimeSheet(year, weekNo, null);
					IsRefreshRequired = false;
				}
				if (timeSheetResponse != null && timeSheetResponse.ResponseStatus == DataServices.Requests.ResponseStatus.SuccessWithResult)
				{
					FormatTimeSheet(timeSheetResponse.Result, selectedDay);
				}
			}
			catch (Exception ex)
			{

			}
			finally
			{
				IsBusy = false;
			}

		}

		private void FormatTimeSheet(TimeSheet timeSheet, DateTime selectedDay)
		{
			NoProjectsExists = false;
			TimeSheetFormated = new TimeSheetFormated();
			TimeSheetFormated.Projects = new List<Project>();
			List<TimeSheetDetails> currentDayTasks = new List<TimeSheetDetails>();

			//Get All Project across the week
			var allProjects = timeSheet.TimeSheetRecords.Where(a => string.IsNullOrEmpty(a.SubTaskTo));

			//Get All Tasks Give a Day
			if (selectedDay.Date.ToString("dddd") == AppResources.Sunday)
			{
				currentDayTasks = timeSheet.TimeSheetRecords.Where(a => !string.IsNullOrEmpty(a.Sunday)).ToList();
			}
			else if (selectedDay.Date.ToString("dddd") == AppResources.Monday)
			{
				currentDayTasks = timeSheet.TimeSheetRecords.Where(a => !string.IsNullOrEmpty(a.Monday)).ToList();
			}
			else if (selectedDay.Date.ToString("dddd") == AppResources.Tuesday)
			{
				currentDayTasks = timeSheet.TimeSheetRecords.Where(a => !string.IsNullOrEmpty(a.Tuesday)).ToList();
			}
			else if (selectedDay.Date.ToString("dddd") == AppResources.Wednesday)
			{
				currentDayTasks = timeSheet.TimeSheetRecords.Where(a => !string.IsNullOrEmpty(a.Wednesday)).ToList();
			}
			else if (selectedDay.Date.ToString("dddd") == AppResources.Thursday)
			{
				currentDayTasks = timeSheet.TimeSheetRecords.Where(a => !string.IsNullOrEmpty(a.Thursday)).ToList();
			}

			foreach (var task in currentDayTasks)
			{
				Project existingProject = TimeSheetFormated.Projects.FirstOrDefault(a => a.Id == task.AssignmentID);
				if (existingProject != null)
				{
					var dayOfWeek = DateTime.Now.ToString("dddd");
					var day = GetProjectTaskDuration(task, selectedDay.Date.ToString("dddd"));
					bool canEdit = isBetween(selectedDay.Date);
					var newTask = new ProjectTask
					{
						Id = task.TaskID,
						Name = task.TaskTitle,
						CanEdit = canEdit,
						Day = day,
						AssigmentId = existingProject.Id,
						AssigmentName = existingProject.Name,
					};
					existingProject.Tasks.Add(newTask);
				}
				else
				{
					var project = allProjects.FirstOrDefault(a => a.AssignmentID == task.AssignmentID);

					var newTasks = new List<ProjectTask>();
					var dayOfWeek = DateTime.Now.ToString("dddd");
					var day = GetProjectTaskDuration(task, selectedDay.Date.ToString("dddd"));
					bool canEdit = isBetween(selectedDay.Date);
					var newTask = new ProjectTask
					{
						Id = task.TaskID,
						Name = task.TaskTitle,
						CanEdit = canEdit,
						Day = day,
						AssigmentId = project.AssignmentID,
						AssigmentName = project.TaskTitle,
					};
					newTasks.Add(newTask);

					var newProject = new Project
					{
						Id = project.AssignmentID,
						Name = project.TaskTitle,
						Tasks = newTasks,
					};
					TimeSheetFormated.Projects.Add(newProject);
				}
			}

			foreach (var project in TimeSheetFormated.Projects)
			{
				project.TotalHours = project.Tasks.Sum(a => a.Day.Hours);
			}

			if (TimeSheetFormated.Projects != null && TimeSheetFormated.Projects.Count > 0)
			{
				TimeSheetFormated.LoggedInHours = TimeSheetFormated.Projects.Sum(a => a.TotalHours);
				var number = (int)TimeSheetFormated.LoggedInHours;
				var dec = TimeSheetFormated.LoggedInHours - number;
				if (dec > 0.3)
				{
					TimeSheetFormated.LoggedInHours = number + 1;
				}
				if (TimeSheetFormated.LoggedInHours > 8)
				{
					TimeSheetFormated.RemainingHours = 0;
				}
				else {

					TimeSheetFormated.RemainingHours = 8 - TimeSheetFormated.LoggedInHours;
					number = (int)TimeSheetFormated.RemainingHours;
					dec = TimeSheetFormated.RemainingHours - number;
					if (dec > 0)
					{
						TimeSheetFormated.RemainingHours = number + 0.3;
					}
				}
			}
			else
			{
				TimeSheetFormated.RemainingHours = 8;
			}
			GroupedTimeSheet = new ObservableCollection<Grouping<Project, ProjectTask>>();
			foreach (var item in TimeSheetFormated.Projects)
			{
				GroupedTimeSheet.Add(new Grouping<Project, ProjectTask>(item, item.Tasks));
			}
			if (GroupedTimeSheet != null && GroupedTimeSheet.Count > 0)
			{
				NoProjectsExists = false;
				ProjectsExists = true;
			}
			else
			{
				NoProjectsExists = true;
				ProjectsExists = false;
			}

		}

		DayWithLoggedInHours GetProjectTaskDuration(TimeSheetDetails task, string selectedDay)
		{
			if (selectedDay != AppResources.Sunday)
			{
				if (selectedDay != AppResources.Monday)
				{
					if (selectedDay != AppResources.Tuesday)
					{
						if (selectedDay != AppResources.Wednesday)
						{
							if (selectedDay != AppResources.Thursday)
							{
								return null;
							}
							else
							{
								return new DayWithLoggedInHours { Hours = double.Parse(task.Thursday), DayName = AppResources.Thursday, Comment = task.ThursdayComment };
							}
						}
						else
						{
							return new DayWithLoggedInHours { Hours = double.Parse(task.Wednesday), DayName = AppResources.Wednesday, Comment = task.WednesdayComment };
						}
					}
					else
					{
						return new DayWithLoggedInHours { Hours = double.Parse(task.Tuesday), DayName = AppResources.Tuesday, Comment = task.TuesdayComment };
					}
				}
				else
				{
					return new DayWithLoggedInHours { Hours = double.Parse(task.Monday), DayName = AppResources.Monday, Comment = task.MondayComment };
				}
			}
			else
			{
				return new DayWithLoggedInHours { Hours = double.Parse(task.Sunday), DayName = AppResources.Sunday, Comment = task.SundayComment };
			}

		}

		#endregion

		#endregion

	}
}
