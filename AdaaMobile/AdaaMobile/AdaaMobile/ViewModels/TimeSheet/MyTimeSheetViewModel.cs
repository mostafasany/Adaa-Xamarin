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
        #endregion

        #region Methods

        private async Task Loaded()
        {
            try
            {
                if (WeekList != null && WeekList.Count > 0)
                    return;
                GroupedTimeSheet = new ObservableCollection<Grouping<Project, ProjectTask>>();
                NoProjectsExists = true;
                ProjectsExists = false;

                IsBusy = true;

                var response = await _dataService.GetWeeksPerYearAsync(2016, null);
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
                if (SelectedProjectTask.CanEdit)
                {
                    _navigationService.NavigateToPage(typeof(EditTask));
                }
                else
                {
                    _navigationService.NavigateToPage(typeof(TaskDetails));
                }
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

                if (Locator.Default.AppSettings.SelectedCultureName.Contains("ar"))
                {
                    //Get first item
                    var firstDay = daysList.LastOrDefault();
                    SelecteDay(firstDay);
                }
                else
                {
                    //Get first item
                    var firstDay = daysList.FirstOrDefault();
                    SelecteDay(firstDay);
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

        public async void SelecteDay(DayWrapper day)
        {
            try
            {
                GroupedTimeSheet = new ObservableCollection<Grouping<Project, ProjectTask>>();
                NoProjectsExists = true;
                ProjectsExists = false;
                IsAddTaskButtonVisible = day.Date.Day==DateTime.Now.Day;
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
                    await LoadTimeSheet(SelectedWeek.WeekNumber, DateTime.Now.Year, SelectedDay.Date);

            }
            catch (Exception ex)
            {
            }
            finally
            {
                IsBusy = false;
            }



        }

        #endregion

        #region TimeSheet

        public async Task LoadTimeSheet(int weekNo, int year, DateTime selectedDay)
        {
            try
            {
                IsBusy = true;
                timeSheetResponse = await _dataService.GetTimeSheet(year, 33, null);
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

        private void FormatTimeSheet(TimeSheet timeSheet, DateTime selectedWeekDate)
        {
            NoProjectsExists = false;
            TimeSheetFormated = new TimeSheetFormated();
            TimeSheetFormated.Projects = new List<Project>();
            var currentDayTimeSheet = timeSheet.TimeSheetRecords.Where
                                               (a => !string.IsNullOrEmpty(a.AssignmentDate) &&
                                                DateTime.Parse(a.AssignmentDate).Day == selectedWeekDate.Day);
            var allProjects = currentDayTimeSheet.Where(a => string.IsNullOrEmpty(a.SubTaskTo));

            foreach (var project in allProjects)
            {
                var newProject = new Project
                {
                    Id = project.AssignmentID,
                    Name = project.TaskTitle,
                    Tasks = new List<ProjectTask>(),
                };
                var projectTasks = currentDayTimeSheet.Where(a => a.AssignmentID == newProject.Id && !string.IsNullOrEmpty(a.SubTaskTo));
                foreach (var task in projectTasks)
                {
                    var dayOfWeek = new DateTime().ToString("dddd");
                    var day = GetProjectTaskDuration(task);
                    var newTask = new ProjectTask
                    {
                        Id = task.TaskID,
                        Name = task.TaskTitle,
                        CanEdit = dayOfWeek == (day != null ? day.DayName : ""),
                        Day = day,
                        AssigmentId = project.AssignmentID,
                        AssigmentName = project.TaskTitle,
                    };
                    newProject.Tasks.Add(newTask);
                }
                if (newProject.Tasks != null && newProject.Tasks.Count > 0)
                {
                    newProject.TotalHours = newProject.Tasks.Sum(a => a.Day.Hours);
                    TimeSheetFormated.Projects.Add(newProject);
                }
            }
            if (TimeSheetFormated.Projects != null && TimeSheetFormated.Projects.Count > 0)
            {
                TimeSheetFormated.LoggedInHours = TimeSheetFormated.Projects.Sum(a => a.TotalHours);
                TimeSheetFormated.RemainingHours = 8 - TimeSheetFormated.LoggedInHours;
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

        DayWithLoggedInHours GetProjectTaskDuration(TimeSheetDetails task)
        {
            if (String.IsNullOrEmpty(task.Sunday))
            {
                if (String.IsNullOrEmpty(task.Monday))
                {
                    if (String.IsNullOrEmpty(task.Tuesday))
                    {
                        if (String.IsNullOrEmpty(task.Wednesday))
                        {
                            if (String.IsNullOrEmpty(task.Thursday))
                            {
                                return null;
                            }
                            else
                            {
                                return new DayWithLoggedInHours { Hours = double.Parse(task.Thursday), DayName = "Thursday", Comment = task.ThursdayComment };
                            }
                        }
                        else
                        {
                            return new DayWithLoggedInHours { Hours = double.Parse(task.Wednesday), DayName = "Wednesday", Comment = task.WednesdayComment };
                        }
                    }
                    else
                    {
                        return new DayWithLoggedInHours { Hours = double.Parse(task.Tuesday), DayName = "Tuesday", Comment = task.TuesdayComment };
                    }
                }
                else
                {
                    return new DayWithLoggedInHours { Hours = double.Parse(task.Monday), DayName = "Monday", Comment = task.MondayComment };
                }
            }
            else
            {
                return new DayWithLoggedInHours { Hours = double.Parse(task.Sunday), DayName = "Sunday", Comment = task.SundayComment };
            }

        }

        #endregion

        #endregion

    }
}
