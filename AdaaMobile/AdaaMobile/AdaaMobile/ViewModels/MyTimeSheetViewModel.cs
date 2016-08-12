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

namespace AdaaMobile.ViewModels
{
    public class MyTimeSheetViewModel : BindableBase
    {
        #region Fields

        private readonly INavigationService _navigationService;
        private readonly IDataService _dataService;
        private readonly int SmallestWindowLimit;
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
            RequestItemSelectedCommand = new AsyncExtendedCommand<TimeSheetDetails>(OpenRequestDetailsPage);
            SmallestWindowLimit = Xamarin.Forms.Device.OnPlatform(5, 14, 14);
        }

        #endregion

        #region Commands

        public AsyncExtendedCommand PageLoadedCommand { get; set; }
        public AsyncExtendedCommand AddNewTaskCommand { get; set; }
        public AsyncExtendedCommand<TimeSheetDetails> RequestItemSelectedCommand { get; set; }
        #endregion

        #region Methods

        private async Task OpenRequestDetailsPage(TimeSheetDetails pendingTask)
        {
            //SelectedPendingTask = pendingTask;
            //_navigationService.NavigateToPage(typeof(SelectedPendingTaskPage));
        }

        private async Task AddNewTask()
        {
            _navigationService.NavigateToPage(typeof(AddTask));
        }
        private async Task Loaded()
        {
            LoadWeeks();

            PopulateAttendanceDaysAsync();

            LoadTimeSheet();
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
            var response = await _dataService.GetTimeSheet(2016, 33, null);
            if (response != null && response.ResponseStatus == DataServices.Requests.ResponseStatus.SuccessWithResult)
            {
                TimeSheetFormated = FormatTimeSheet(response.Result);
                GroupedTimeSheet = new ObservableCollection<Grouping<Project, ProjectTask>>();
                foreach (var item in TimeSheetFormated.Projects)
                {
                    GroupedTimeSheet.Add(new Grouping<Project, ProjectTask>(item, item.Tasks));
                }
                //TimeSheet _TimeSheetItem = response.Result;
                //var sorted = from emp in TimeSheetFormated.Projects
                //             group emp by emp.Tasks into empGroup
                //             select new Grouping<Project, List<ProjectTask>>(null, empGroup.ToList());

                //GroupedTimeSheet = new ObservableCollection<Grouping<string, TimeSheetDetails>>(sorted);

            }
        }

        private TimeSheetFormated FormatTimeSheet(TimeSheet timeSheet)
        {
            var formatedTimeSheet = new TimeSheetFormated();
            formatedTimeSheet.Projects = new List<Project>();

            var allProjects = timeSheet.TimeSheetRecords.Where(a => string.IsNullOrEmpty(a.SubTaskTo));

            foreach (var project in allProjects)
            {
                var newProject = new Project
                {
                    Id = project.AssignmentID,
                    Name = project.TaskTitle,
                    Tasks = new List<ProjectTask>(),
                };
                var projectTasks = timeSheet.TimeSheetRecords.Where(a => a.AssignmentID == newProject.Id && !string.IsNullOrEmpty(a.SubTaskTo));
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
                    formatedTimeSheet.Projects.Add(newProject);
                }

            }
            return formatedTimeSheet;
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
        /// <summary>
        /// This is used to populate days list with the specified list.
        /// Be Aware this is used only With Attendance Mode.
        /// Use LoadExceptions with Exceptions Mode
        /// </summary>
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

        /// <summary>
        /// Add dummy days to the list to match the minimum required windows limit.
        /// Mainly UI related.
        /// </summary>
        /// <param name="daysList"></param>
        private void FixWindowSize(List<DayWrapper> daysList)
        {
            if (daysList.Count < SmallestWindowLimit)
            {
                for (int i = daysList.Count; i < SmallestWindowLimit; i++)
                {
                    daysList.Add(new DayWrapper(new DateTime(0), true));
                }
            }
        }

        /// <summary>
        /// It's used primary used to selecting day by default when range is loaded.
        /// </summary>
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


        }
        #endregion

    }
}
