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

        private PendingTask _SelectedTask;

        public PendingTask SelectedTask
        {
            get { return _SelectedTask; }
            set { SetProperty(ref _SelectedTask, value); }
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

        /// <summary>
        /// This is the selected day.
        /// </summary>
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

        #endregion

        #region Initialization

        public MyTimeSheetViewModel(INavigationService navigationService, IDataService dataservice)
        {
            _navigationService = navigationService;
            _dataService = dataservice;
            PageLoadedCommand = new AsyncExtendedCommand(Loaded);
            AddNewTaskCommand = new AsyncExtendedCommand(AddNewTask);
            RequestItemSelectedCommand = new AsyncExtendedCommand<object>(OpenRequestDetailsPage);
            SmallestWindowLimit = Xamarin.Forms.Device.OnPlatform(5, 14, 14);
        }

        #endregion

        #region Commands

        public AsyncExtendedCommand PageLoadedCommand { get; set; }
        public AsyncExtendedCommand AddNewTaskCommand { get; set; }
        public AsyncExtendedCommand<object> RequestItemSelectedCommand { get; set; }
        #endregion

        #region Methods

        private async Task Loaded()
        {
            LoadWeeks();

            PopulateAttendanceDaysAsync();

            LoadTimeSheet();
        }

        private async Task OpenRequestDetailsPage(object projectTask)
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
                    //Should Go to Task Details Page
                    //_navigationService.NavigateToPage(typeof(EditTask));
                }
            }

        }

        private async Task AddNewTask()
        {
            _navigationService.NavigateToPage(typeof(AddTask));
        }

        public async Task LoadWeeks()
        {
            var response = await _dataService.GetWeeksPerYearAsync(2016, null);
            if (response != null && response.ResponseStatus == DataServices.Requests.ResponseStatus.SuccessWithResult)
            {
                WeekList = response.Result;
                SelectedWeek = WeekList[0];
            }
        }

        public async Task LoadTimeSheet()
        {
            timeSheetResponse = await _dataService.GetTimeSheet(2016, 33, null);
            if (timeSheetResponse != null && timeSheetResponse.ResponseStatus == DataServices.Requests.ResponseStatus.SuccessWithResult)
            {
                FormatTimeSheet(timeSheetResponse.Result, SelectedDay.Date);
            }
        }

        private void FormatTimeSheet(TimeSheet timeSheet, DateTime selectedWeekDate)
        {
            TimeSheetFormated = new TimeSheetFormated();
            TimeSheetFormated.Projects = new List<Project>();
			var currentDayTimeSheet= timeSheet.TimeSheetRecords.Where(a => a.AssignmentDate== selectedWeekDate.ToString());
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

        public async Task PopulateAttendanceDaysAsync()
        {
            //Create new Days List on background task.
            List<DayWrapper> daysList = await Task.Run(() =>
            {
                var days = new List<DayWrapper>();
                var currentDate = new DateTime();
                var endDate = new DateTime().AddDays(7);
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

            //Select first Day
            SelectDayAfterRangeLoad();
        }

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

        private void SelectDayAfterRangeLoad()
        {
            if (DaysList == null)
                return;
            var daysList = DaysList;

            var nowDate = DateTime.Now;
            var currentDay = daysList.FirstOrDefault(a => a.Date.Date == nowDate.Date);
            if (currentDay != null)
            {
                SelecteDay(currentDay);
            }
            else
            {

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
        }

        public void SelecteDay(DayWrapper day)
        {
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
                FormatTimeSheet(timeSheetResponse.Result, SelectedDay.Date);

        }

        #endregion

    }
}
