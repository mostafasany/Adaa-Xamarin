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
        public DateTime MinimumDate
        {
            get { return _minimumDate; }
            set { _minimumDate = value; }
        }

        private List<DayWrapper> _daysList;
        public List<DayWrapper> DaysList
        {
            get { return _daysList; }
            set { SetProperty(ref _daysList, value); }
        }

        private DayWrapper _selectedDay;
        public DayWrapper SelectedDay
        {
            get { return _selectedDay; }
            set { SetProperty(ref _selectedDay, value); }
        }

        private Attendance _currentAttendance;
        public Attendance CurrentAttendance
        {
            get { return _currentAttendance; }
            set { SetProperty(ref _currentAttendance, value); }
        }


        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }

        private string _busyMessage;
        public string BusyMessage
        {
            get { return _busyMessage; }
            set { SetProperty(ref _busyMessage, value); }
        }

        private string _errorMessage;
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
            PopulateDays();
        }

        private async void PopulateDays()
        {
            var daysList = await Task.Run<List<DayWrapper>>(() =>
               {
                   var days = new List<DayWrapper>();
                   var currentDate = StartDate;
                   var endDate = EndDate;
                   //Limit large unneccessary range if any
                   // if ((EndDate - StartDate).Days > LimitRangeInDays) endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 0, 0, 0);
                   while (currentDate <= endDate)
                   {
                       days.Add(new DayWrapper(currentDate));
                       currentDate = currentDate.AddDays(1);
                   }
                   return days;
               });

            DaysList = daysList;

            //Initialize commands
            LoadAttendanceCommand = new AsyncExtendedCommand(LoadDayDetailsAsync);
        }
        #endregion

        #region Commands

        public AsyncExtendedCommand LoadAttendanceCommand { get; set; }
        #endregion

        #region Methods

        private async Task LoadDayDetailsAsync(CancellationToken token)
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

        #endregion

    }
}
