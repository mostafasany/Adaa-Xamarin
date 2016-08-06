using AdaaMobile.DataServices;
using AdaaMobile.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdaaMobile.Common;
using AdaaMobile.Models;
using System.Collections.ObjectModel;
using AdaaMobile.Models.Response;

namespace AdaaMobile.ViewModels
{
	public class MyTimeSheetViewModel: BindableBase
	{
		#region Fields

		private readonly INavigationService _navigationService;
		private readonly IDataService _dataService;
		private readonly int SmallestWindowLimit;
		#endregion

		#region Properties

		public ObservableCollection<Assignment> Assignments { get; private set; }

		private List<DayWrapper> _daysList;

		public List<DayWrapper> DaysList {
			get { return _daysList; }
			set { SetProperty (ref _daysList, value); }
		}

		private DayWrapper _selectedDay;

		/// <summary>
		/// This is the selected day.
		/// </summary>
		public DayWrapper SelectedDay {
			get { return _selectedDay; }
			set { SetProperty (ref _selectedDay, value); }
		}

		Assignment _SelectedAssignment;
		public Assignment SelectedAssignment {
			get { return _SelectedAssignment; }
			set { SetProperty (ref _SelectedAssignment, value); }
		}

		string _SelectedAssignmentResult;
		public string SelectedAssignmentResult {
			get { return _SelectedAssignmentResult; }
			set { SetProperty (ref _SelectedAssignmentResult, value); }
		}

		PendingTask _SelectedPendingTask;
		public PendingTask SelectedPendingTask {
			get { return _SelectedPendingTask; }
			set { SetProperty (ref _SelectedPendingTask, value); }
		}

		string _SelectedPendingTaskResult;
		public string SelectedPendingTaskResult {
			get { return _SelectedPendingTaskResult; }
			set { SetProperty (ref _SelectedPendingTaskResult, value); }
		}

		string _SelectedDuration;
		public string SelectedDuration {
			get { return _SelectedDuration; }
			set { SetProperty (ref _SelectedDuration, value); }
		}
			

		#endregion

		#region Initialization

		public MyTimeSheetViewModel (INavigationService navigationService,IDataService dataservice)
		{
			_navigationService = navigationService;
			_dataService = dataservice;
			PageLoadedCommand = new AsyncExtendedCommand (Loaded);
			AddNewTaskCommand = new AsyncExtendedCommand (AddNewTask);
			SmallestWindowLimit = Xamarin.Forms.Device.OnPlatform (5, 14, 14);
		}

		#endregion

		#region Commands

		public AsyncExtendedCommand PageLoadedCommand { get; set; }
		public AsyncExtendedCommand AddNewTaskCommand { get; set; }

		#endregion

		#region Methods

		private async Task AddNewTask ()
		{
			_navigationService.NavigateToPage (typeof(AddTask));
		}
		private async Task Loaded ()
		{
			SelectedDuration="Selecte Duration";
			SelectedPendingTaskResult="Selecte  Task Name";
			SelectedAssignmentResult="Selecte  Assignment";

			PopulateAttendanceDaysAsync ();
		}

		/// <summary>
		/// This is used to populate days list with the specified list.
		/// Be Aware this is used only With Attendance Mode.
		/// Use LoadExceptions with Exceptions Mode
		/// </summary>
		public async Task PopulateAttendanceDaysAsync ()
		{
			//Create new Days List on background task.
			List<DayWrapper> daysList = await Task.Run (() => {
				var days = new List<DayWrapper> ();
				var currentDate = new DateTime();
				var endDate = new DateTime().AddDays(7);
				while (currentDate <= endDate) {
					if (currentDate.DayOfWeek != DayOfWeek.Friday && currentDate.DayOfWeek != DayOfWeek.Saturday)
						days.Add (new DayWrapper (currentDate, false));
					currentDate = currentDate.AddDays (1);
				}
				return days;
			});

			if (Locator.Default.AppSettings.SelectedCultureName.Contains ("ar")) {
				daysList.Reverse ();
			} 

			FixWindowSize (daysList);


			DaysList = daysList;

			//Select first Day
			SelectDayAfterRangeLoad ();
		}

		/// <summary>
		/// Add dummy days to the list to match the minimum required windows limit.
		/// Mainly UI related.
		/// </summary>
		/// <param name="daysList"></param>
		private void FixWindowSize (List<DayWrapper> daysList)
		{
			if (daysList.Count < SmallestWindowLimit) {
				for (int i = daysList.Count; i < SmallestWindowLimit; i++) {
					daysList.Add (new DayWrapper (new DateTime (0), true));
				}
			}
		}

		/// <summary>
		/// It's used primary used to selecting day by default when range is loaded.
		/// </summary>
		private void SelectDayAfterRangeLoad ()
		{
			if (DaysList == null)
				return;
			var daysList = DaysList;

			var nowDate = DateTime.Now;
			var currentDay = daysList.FirstOrDefault (a => a.Date.Date == nowDate.Date);
			if (currentDay != null) {
				SelecteDay (currentDay);
			} else {

				if (Locator.Default.AppSettings.SelectedCultureName.Contains ("ar")) {
					//Get first item
					var firstDay = daysList.LastOrDefault ();
					SelecteDay (firstDay);
				} else {
					//Get first item
					var firstDay = daysList.FirstOrDefault ();
					SelecteDay (firstDay);
				}

			}
		}

		public void SelecteDay (DayWrapper day)
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
