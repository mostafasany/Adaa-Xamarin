using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AdaaMobile.Common;
using AdaaMobile.DataServices;
using AdaaMobile.DataServices.Requests;
using AdaaMobile.Helpers;
using AdaaMobile.Models;
using AdaaMobile.Models.Request;
using AdaaMobile.Models.Response;
using AdaaMobile.Strings;
using AdaaMobile.Views.Authentication;
using System.Collections.ObjectModel;

namespace AdaaMobile.ViewModels
{
	public class AttendanceViewModel : BindableBase
	{

		#region Fields

		private const int InitialLimitInDays = 7;
		private const int LimitRangeInDays = 1 * 30;
		private const int BottomLimitInDays = 2 * 12 * 30;
		private readonly int SmallestWindowLimit;
		//items
		private readonly IDataService _dataService;
		private readonly IAppSettings _appSettings;
		private readonly IRequestMessageResolver _messageResolver;
		private readonly IDialogManager _dialogManager;
		private readonly INavigationService _navigationService;

		#endregion

		#region Properties

		public bool isSubordinateDataShown = false;
		public Employee SelectedSub;
		private DateTime _startDate = DateTime.Now.Subtract (TimeSpan.FromDays (InitialLimitInDays));

		/// <summary>
		/// Date of first picker, this date will be used to show the starting range for Attendance and Exceptions.
		/// </summary>
		public DateTime StartDate {
			get { return _startDate; }
			set {
				if (SetProperty (ref _startDate, value)) {
					if ((_endDate >= _startDate && (_endDate - _startDate).Days > LimitRangeInDays) || _startDate >= _endDate) {
						_endDate = _startDate.AddDays (LimitRangeInDays);
						OnPropertyChanged ("EndDate");
					}
					//                    var span = EndDate - StartDate;
					//                    if (span.Days > LimitRangeInDays)
					//                        EndDate = _startDate.Add(TimeSpan.FromDays(LimitRangeInDays));
					SwitchMode (AttendanceMode);
				}
			}
		}

		private DateTime _endDate = DateTime.Now;

		/// <summary>
		/// Date of second picker, this date will be used to show the end of range for Attendance and Exceptions.
		/// </summary>
		public DateTime EndDate {
			get { return _endDate; }
			set {
				if (SetProperty (ref _endDate, value)) {
					if ((_endDate >= _startDate && (_endDate - _startDate).Days > LimitRangeInDays)) {
						_startDate = _endDate.AddDays (LimitRangeInDays * -1);
						OnPropertyChanged ("StartDate");
					}
					//                    var span = EndDate - StartDate;
					//                    if (span.Days > LimitRangeInDays)
					//                        StartDate = _endDate.Subtract(TimeSpan.FromDays(LimitRangeInDays));
					SwitchMode (AttendanceMode);
				}
			}
		}

		private DateTime _minimumDate = DateTime.Now.Subtract (TimeSpan.FromDays (BottomLimitInDays));

		/// <summary>
		/// First picker can't go below this date.
		/// </summary>
		public DateTime MinimumDate {
			get { return _minimumDate; }
			set { _minimumDate = value; }
		}

		private DateTime _maximum;

		public DateTime Maximum {
			get { return _maximum; }
			set { SetProperty (ref _maximum, value); }
		}


		private AttendanceMode _attendanceMode;

		/// <summary>
		/// This represents the current mode whether we are showing attendance or Exceptions.
		/// </summary>
		public AttendanceMode AttendanceMode {
			get { return _attendanceMode; }
			set { SetProperty (ref _attendanceMode, value); }
		}


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

		private Attendance _currentAttendance;

		/// <summary>
		/// Current Attendance data for selected day.
		/// </summary>
		public Attendance CurrentAttendance {
			get { return _currentAttendance; }
			set { SetProperty (ref _currentAttendance, value); }
		}

		//TODO:Decide to use one property to report progress or two seperate properties
		private bool _isBusy;

		/// <summary>
		/// Boolean to indicate an ongoin operation.
		/// </summary>
		public bool IsBusy {
			get { return _isBusy; }
			set { SetProperty (ref _isBusy, value); }
		}

		private string _busyMessage;

		/// <summary>
		/// Loading message.
		/// </summary>
		public string BusyMessage {
			get { return _busyMessage; }
			set { SetProperty (ref _busyMessage, value); }
		}

		private string _errorMessage;

		/// <summary>
		/// Will be used to report errors.
		/// </summary>
		public string ErrorMessage {
			get { return _errorMessage; }
			set { SetProperty (ref _errorMessage, value); }
		}




		private ObservableCollection<Employee> _SubordinateList;

		public ObservableCollection<Employee> SubordinateList {
			get { return _SubordinateList; }
			set { SetProperty (ref _SubordinateList, value); }
		}

		#endregion

		#region Initialization

		public AttendanceViewModel (IDataService dataService, IAppSettings appSettings, IRequestMessageResolver messageResolver, IDialogManager dialogManager, INavigationService navigationService)
		{
			_dataService = dataService;
			_appSettings = appSettings;
			_messageResolver = messageResolver;
			_dialogManager = dialogManager;
			_navigationService = navigationService;

			//_endDate = DateTime.Now;
			//_startDate = _endDate.Subtract(TimeSpan.FromDays(14));

			SmallestWindowLimit = Xamarin.Forms.Device.OnPlatform (5, 14, 14);

			//Initialize commands
			LoadAttendanceCommand = new AsyncExtendedCommand (LoadAttendanceDetailsAsync);
			LoadExceptionsCommand = new AsyncExtendedCommand (LoadExceptionsAsync);

			//Set initial mode to Attendance
			AttendanceMode = AttendanceMode.Attendance;

		}

		#endregion

		#region Commands

		public AsyncExtendedCommand LoadAttendanceCommand { get; set; }

		public AsyncExtendedCommand LoadExceptionsCommand { get; set; }

		#endregion

		#region Methods

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
				var currentDate = StartDate;
				var endDate = EndDate;
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

			//Load data for this day
			if (AttendanceMode == AttendanceMode.Attendance) {
				LoadAttendanceCommand.Execute (null);
			} else {
				//Do nothing, dayWrapper for exception already has data
			}
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

		public async Task SwitchMode (AttendanceMode mode)
		{
			if (EndDate >= StartDate && (EndDate - StartDate).Days > LimitRangeInDays) {
				//await _dialogManager.DisplayAlert (AppResources.ApplicationName, "Please select valid intreval, maximum is one month", AppResources.Ok);

				return;
			}
			IsBusy = true;

			try {
				//Set current mode, This will trigger changes in Bindings.
				AttendanceMode = mode;

				//Set Busy message to null
				BusyMessage = null;

				//Clear any errors from previous attempt to loading
				ErrorMessage = null;

				DaysList = null;


				if (SelectedDay != null)
            //Clear current day
            SelectedDay.IsSelected = false;
				SelectedDay = null;

				//Clear current Attendance
				CurrentAttendance = null;

				//Cancel loading Exceptions command if any
				LoadExceptionsCommand.Cancel ();

				//Cancel Loading attendance command if any
				LoadAttendanceCommand.Cancel ();


				if (mode == AttendanceMode.Attendance) {
					await PopulateAttendanceDaysAsync ();
					IsBusy = false;

				} else {
					//Load Exception Details for selected day.
					LoadExceptionsCommand.Execute (null);
				}
			} catch (Exception ex) {
				
			} finally {
			}
		}

		private async Task LoadAttendanceDetailsAsync (CancellationToken token)
		{
			if (SelectedDay == null)
				return;
			try {
				IsBusy = true;
				BusyMessage = AppResources.Loading;
				ErrorMessage = null;
				CurrentAttendance = null;

				if (token.IsCancellationRequested)
					return;
				var paramters = new AttendanceQParameters () {
					Date = SelectedDay.Date,
					Langid = _appSettings.Language,
					UserToken = _appSettings.UserToken
				};

				if (SelectedSub != null) {
					paramters.subordinateID = SelectedSub.UserId;
				}
				var response = await _dataService.GetAttendanceRecordAsync (paramters, token);

				if (token.IsCancellationRequested)
					return;

				if (response.ResponseStatus == ResponseStatus.SuccessWithResult) {
					CurrentAttendance = response.Result;
				} else {
					string message = _messageResolver.GetMessage (response);
					await _dialogManager.DisplayAlert (AppResources.ApplicationName, message, AppResources.Ok);

					if (response.ResponseStatus == ResponseStatus.InvalidToken) {
						_navigationService.SetAppCurrentPage (typeof(LoginPage));
					}
				}

			} catch (Exception) {
#pragma warning disable 4014
				_dialogManager.DisplayAlert (AppResources.ApplicationName, AppResources.LoadingError, AppResources.Ok);
#pragma warning restore 4014
			} finally {
				//Don't set IsBusy to false on cancel from here.
				//As this might interchange with the newer request
				//Instead set IsBusy manually before calling cancel method
				if (!token.IsCancellationRequested) {
					IsBusy = false;
					BusyMessage = null;
				}
			}

		}

		/// <summary>
		/// Load Exceptions happened in Specified Range.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		private async Task LoadExceptionsAsync (CancellationToken token)
		{
			try {
				IsBusy = true;
				BusyMessage = AppResources.Loading;
				ErrorMessage = null;

				if (token.IsCancellationRequested)
					return;

				//Show dummy list till the data is loaded
				DaysList = new List<DayWrapper> ();
				FixWindowSize (DaysList);

				var paramters = new ExceptionsQParameter () {
					StartDate = StartDate,
					EndDate = EndDate,
					Langid = _appSettings.Language,
					UserToken = _appSettings.UserToken
				};
				if (SelectedSub != null) {
					paramters.subordinateID = SelectedSub.UserId;
				}
				var response = await _dataService.GetAttendanceExceptionsAsync (paramters, token);

				if (token.IsCancellationRequested)
					return;

				if (response.ResponseStatus == ResponseStatus.SuccessWithResult) {
					if (response.Result.ExceptionDays != null) {
						if (response.Result.ExceptionDays.Count () == 1) {
							if (response.Result.ExceptionDays [0].Message.ToLower () == "nv") {
								return;
							}
						}
						var daysList = response.Result.ExceptionDays.Select (ex => (DayWrapper)ExceptionDayWrapper.Wrap (ex)).ToList ();
						if (Locator.Default.AppSettings.SelectedCultureName.Contains ("ar")) {
							daysList.Reverse ();
						} 
						FixWindowSize (daysList);
						DaysList = daysList;
					}

					//Select first day
					SelectDayAfterRangeLoad ();
				} else {
					string message = _messageResolver.GetMessage (response);
					await _dialogManager.DisplayAlert (AppResources.ApplicationName, message, AppResources.Ok);
					if (response.ResponseStatus == ResponseStatus.InvalidToken) {
						_navigationService.SetAppCurrentPage (typeof(LoginPage));
					}
				}

			} catch (Exception) {
#pragma warning disable 4014
				_dialogManager.DisplayAlert (AppResources.ApplicationName, AppResources.LoadingError, AppResources.Ok);
#pragma warning restore 4014
			} finally {
				//Don't set IsBusy to false on cancel from here.
				//As this might interchange with the newer request
				//Instead set IsBusy manually before calling cancel method
				if (!token.IsCancellationRequested) {
					IsBusy = false;
					BusyMessage = null;
				}
			}
		}


		public async Task<bool> GetSubordinateList ()
		{
			try {
				IsBusy = true;
				var response = await _dataService.GetDelegationSubordinatesResponseAsync (new delegationSubordinatesQParamters () {
					Langid = _appSettings.Language,
					UserToken = _appSettings.UserToken
				});
				if (response.ResponseStatus == ResponseStatus.SuccessWithResult) {
					if (response.Result != null && response.Result.Subordinates != null && response.Result.Subordinates.Length > 0) {
						SubordinateList = new ObservableCollection<Employee> (response.Result.Subordinates);
						SelectedSub = SubordinateList [0];
						SelectedSub.IsSelected = true;
						return true;
					} else {
						return false;
					}
				} else {
					return false;
				}
			} catch (Exception ex) {

				return false;

			} finally {
				IsBusy = false;
			}
		}

		#endregion

	}

	public enum AttendanceMode
	{
		Attendance,
		Exceptions
	}
}
