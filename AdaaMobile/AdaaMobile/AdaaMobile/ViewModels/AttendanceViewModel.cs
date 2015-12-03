using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace AdaaMobile.ViewModels
{
    public class AttendanceViewModel : BindableBase
    {

        #region Fields
        private const int LimitRangeInDays = 2 * 365;
        private readonly IDataService _dataService;
        private readonly IAppSettings _appSettings;
        #endregion

        #region Properties
        private DateTime _startDate;
        /// <summary>
        /// Date of first picker, this date will be used to show the starting range for Attendance and Exceptions.
        /// </summary>
        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                if (SetProperty(ref _startDate, value))
                {
                    PopulateDays();
                }
            }
        }

        private DateTime _endDate;
        /// <summary>
        /// Date of second picker, this date will be used to show the end of range for Attendance and Exceptions.
        /// </summary>
        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                if (SetProperty(ref _endDate, value))
                {
                    PopulateDays();
                }
            }
        }

        private DateTime _minimumDate = DateTime.Now.Subtract(TimeSpan.FromDays(LimitRangeInDays));
        /// <summary>
        /// First picker can't go below this date.
        /// </summary>
        public DateTime MinimumDate
        {
            get { return _minimumDate; }
            set { _minimumDate = value; }
        }

        private AttendanceMode _attendanceMode;
        /// <summary>
        /// This represents the current mode whether we are showing attendance or Exceptions.
        /// </summary>
        public AttendanceMode AttendanceMode
        {
            get { return _attendanceMode; }
            set { SetProperty(ref _attendanceMode, value); }
        }


        private List<DayWrapper> _daysList;
        public List<DayWrapper> DaysList
        {
            get { return _daysList; }
            set { SetProperty(ref _daysList, value); }
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

        private Attendance _currentAttendance;
        /// <summary>
        /// Current Attendance data for selected day.
        /// </summary>
        public Attendance CurrentAttendance
        {
            get { return _currentAttendance; }
            set { SetProperty(ref _currentAttendance, value); }
        }


        private bool _isBusy;
        /// <summary>
        /// Boolean to indicate an ongoin operation.
        /// </summary>
        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }

        private string _busyMessage;
        /// <summary>
        /// Loading message.
        /// </summary>
        public string BusyMessage
        {
            get { return _busyMessage; }
            set { SetProperty(ref _busyMessage, value); }
        }

        private string _errorMessage;
        /// <summary>
        /// Will be used to report errors.
        /// </summary>
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { SetProperty(ref _errorMessage, value); }
        }

        #endregion

        #region Initialization
        public AttendanceViewModel(IDataService dataService, IAppSettings appSettings)
        {
            _dataService = dataService;
            _appSettings = appSettings;

            _startDate = new DateTime(2015, 10, 1, 0, 0, 0);
            _endDate = new DateTime(2015, 10, 31, 0, 0, 0);

            //Set initial mode to Attendance
            SwitchMode(AttendanceMode.Attendance);
        }

        private async void PopulateDays()
        {
            var daysList = await Task.Run<List<DayWrapper>>(() =>
               {
                   var days = new List<DayWrapper>();
                   var currentDate = StartDate;
                   var endDate = EndDate;
                   while (currentDate <= endDate)
                   {
                       days.Add(new DayWrapper(currentDate));
                       currentDate = currentDate.AddDays(1);
                   }
                   return days;
               });

            DaysList = daysList;

            //Initialize commands
            LoadAttendanceCommand = new AsyncExtendedCommand(LoadAttendanceDetailsAsync);
            LoadExceptionsCommand = new AsyncExtendedCommand(LoadExceptionsAsync);
        }
        #endregion

        #region Commands

        public AsyncExtendedCommand LoadAttendanceCommand { get; set; }
        public AsyncExtendedCommand LoadExceptionsCommand { get; set; }
        #endregion

        #region Methods

        private void SwitchMode(AttendanceMode mode)
        {
            //Set current mode, This will trigger changes in Bindings.
            AttendanceMode = mode;
            //Clear Selected Date
            SelectedDay = null;
            //Clear current Attendance
            CurrentAttendance = null;
            //Clear days list
            DaysList = null;
            if (mode == AttendanceMode.Attendance)
            {
                //Cancel loading Exceptions command if any
                LoadExceptionsCommand.Cancel();

                //Populate list with the specified range
                PopulateDays();
            }
            else
            {

                //Cancel Loading attendance command if any
                LoadAttendanceCommand.Cancel();

                //Load Exceptions List
                LoadExceptionsCommand.Execute(null);
            }
        }

        private async Task LoadAttendanceDetailsAsync(CancellationToken token)
        {
            if (SelectedDay == null) return;
            try
            {
                IsBusy = true;
                BusyMessage = AppResources.Loading;
                ErrorMessage = null;
                CurrentAttendance = null;

                if (token.IsCancellationRequested) return;
                var paramters = new AttendanceQParameters()
                {
                    Date = SelectedDay.Date,
                    Langid = _appSettings.Language,
                    UserToken = _appSettings.UserToken
                };
                var response = await _dataService.GetAttendanceRecordAsync(paramters, token);

                if (token.IsCancellationRequested) return;

                if (response.ResponseStatus == ResponseStatus.SuccessWithResult)
                {
                    CurrentAttendance = response.Result;
                }

            }
            catch (Exception)
            {
                ErrorMessage = AppResources.LoadingError;
            }
            finally
            {
                IsBusy = false;
                BusyMessage = null;
            }

        }

        private async Task LoadExceptionsAsync(CancellationToken token)
        {
        }

        #endregion

    }

    public enum AttendanceMode
    {
        Attendance,
        Exceptions
    }
}
