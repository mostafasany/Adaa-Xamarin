using AdaaMobile.Common;
using AdaaMobile.DataServices;
using AdaaMobile.DataServices.Requests;
using AdaaMobile.Helpers;
using AdaaMobile.Models;
using AdaaMobile.Models.Request;
using AdaaMobile.Models.Response;
using AdaaMobile.Strings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaaMobile.ViewModels
{
    /// <summary>
    /// Use this class structure when you create new viewmodel.
    /// </summary>
    public class DayPassViewModel : BindableBase
    {

        #region Fields
        private readonly INavigationService _navigationService;
        private readonly IDataService _dataService;
        private readonly IAppSettings _appSettings;
        private readonly IDialogManager _dialogManager;
        private readonly IRequestMessageResolver _messageResolver;
        #endregion

        #region Properties



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


        private ObservableCollection<DayPassRequest> _pendingRequests;
        public ObservableCollection<DayPassRequest> PendingRequests
        {
            get { return _pendingRequests; }
            set { SetProperty(ref _pendingRequests, value); }
        }


        private bool _showNoPendingRequests;
        public bool ShowNoPendingRequests
        {
            get { return _showNoPendingRequests; }
            set { SetProperty(ref _showNoPendingRequests, value); }
        }

        private ObservableCollection<DayPassTask> _dayPassTasks;
        public ObservableCollection<DayPassTask> DayPassTasks
        {
            get { return _dayPassTasks; }
            set { SetProperty(ref _dayPassTasks, value); }
        }


        private bool _showNoTasks;
        public bool ShowNoTasks
        {
            get { return _showNoTasks; }
            set { SetProperty(ref _showNoTasks, value); }
        }

        private bool _returnToday;
        public bool ReturnToday
        {
            get { return _returnToday; }
            set { SetProperty(ref _returnToday, value); }
        }


        private string _reason;
        public string Reason
        {
            get { return _reason; }
            set { SetProperty(ref _reason, value); }
        }


        private string _startTime;
        public string StartTime
        {
            get { return _startTime; }
            set { SetProperty(ref _startTime, value); }
        }



        private TimeSpan _endTimeSpan;
        public TimeSpan EndTimeSpan
        {
            get { return _endTimeSpan; }
            set { 
				DateTime d = DateTime.Today.Add (value);
				EndTimeSpanString = d.ToString ("hh:mm tt");
				SetProperty(ref _endTimeSpan, value); }
        }

		private string _endTimeSpanString;
		public string EndTimeSpanString
		{
			get { return _endTimeSpanString; }
			set { SetProperty(ref _endTimeSpanString, value); }
		}

        private string _endTime;
        public string EndTime
        {
            get { return _endTime; }
            set { SetProperty(ref _endTime, value); }
        }


        private string _reasonType = "Work";
        public string ReasonType
        {
            get { return _reasonType; }
            set { SetProperty(ref _reasonType, value); }
        }



        private DayPassRequest _selectedRequest;
        public DayPassRequest SelectedRequest
        {
            get { return _selectedRequest; }
            set { SetProperty(ref _selectedRequest, value); }
        }


        private DayPassTask _selectedTask;
        public DayPassTask SelectedTask
        {
            get { return _selectedTask; }
            set { SetProperty(ref _selectedTask, value); }
        }


        private TimeSpan _startTimeSpan;
        public TimeSpan StartTimeSpan
        {
            get { return _startTimeSpan; }
            set { 
				DateTime d = DateTime.Today.Add (value);
				StartTimeSpanString = d.ToString ("hh:mm tt");

				SetProperty(ref _startTimeSpan, value); }
        }
		private string _startTimeSpanString;
		public string StartTimeSpanString
		{
			get { return _startTimeSpanString; }
			set { SetProperty(ref _startTimeSpanString, value); }
		}
        #endregion

        #region Initialization
        public DayPassViewModel(IDataService dataService, IDialogManager dialogManager, IAppSettings appSettings, INavigationService navigationService, IRequestMessageResolver messageResolver)
        {
            _dataService = dataService;
            _appSettings = appSettings;
            _navigationService = navigationService;
            _dialogManager = dialogManager;
            _messageResolver = messageResolver;
            LoadDayPassDataCommand = new AsyncExtendedCommand(LoadDayPassDataAsync);
            NewDayPassCommand = new AsyncExtendedCommand(DoNewDayPassCommand);
			StartTimeSpan = new TimeSpan (0, 0, 0);
			EndTimeSpan = new TimeSpan (0, 0, 0);
        }

        #endregion

        #region Commands
        public AsyncExtendedCommand LoadDayPassDataCommand { get; set; }
        public AsyncExtendedCommand NewDayPassCommand { get; set; }

        #endregion

        #region Methods
        private async Task LoadDayPassDataAsync()
        {
            //Pending requests
            try
            {
                IsBusy = true;
                LoadDayPassDataCommand.CanExecute = false;
                ShowNoPendingRequests = false;
                var response = await _dataService.GetPendingDayPassesAsync(new DayPassesQParameters()
                {
                    Langid = _appSettings.Language,
                    UserToken = _appSettings.UserToken
                });

                if (response.ResponseStatus == ResponseStatus.SuccessWithResult && response.Result != null)
                {
                    if (response.Result.DayPassList != null && response.Result.DayPassList.Length > 0)
                    {
                        PendingRequests = new ObservableCollection<DayPassRequest>(response.Result.DayPassList);
                        ShowNoPendingRequests = false;
                    }
                    else
                    {
                        ShowNoPendingRequests = true;
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }
            finally
            {
                LoadDayPassDataCommand.CanExecute = true;
                IsBusy = false;
            }

            //Get tasks
            try
            {
                IsBusy = true;
                LoadDayPassDataCommand.CanExecute = false;
                ShowNoTasks = false;
                var response = await _dataService.GetDayPassTasksResponseAsync(new DayPassTasksQParameters
                {
                    Langid = _appSettings.Language,
                    UserToken = _appSettings.UserToken
                });

                if (response.ResponseStatus == ResponseStatus.SuccessWithResult && response.Result != null)
                {
                    if (response.Result.DayPassTaskList != null && response.Result.DayPassTaskList.Length > 0)
                    {
                        DayPassTasks = new ObservableCollection<DayPassTask>(response.Result.DayPassTaskList);
                        ShowNoTasks = false;
                    }
                    else
                    {
						DayPassTasks = null;
                        ShowNoTasks = true;
                    }

                }
            }
            catch (Exception)
            {
                // ignored
            }
            finally
            {
                LoadDayPassDataCommand.CanExecute = true;
                IsBusy = false;
            }
        }


        private async Task DoNewDayPassCommand()
        {
            try
            {
                NewDayPassCommand.CanExecute = false;
                IsBusy = true;
                BusyMessage = AppResources.Loading;
                if (string.IsNullOrEmpty(Reason))
                {
                    await _dialogManager.DisplayAlert(AppResources.ApplicationName, AppResources.PleaseEnterReason, AppResources.Ok);
                }
                else
                {
                    var qParamters = new DaypassRequestQParameters()
                    {
                        Langid = _appSettings.Language,
                        UserToken = _appSettings.UserToken,
                        StartTime = StartTimeSpan.ToString(@"hh\:mm"),
                        EndTime = EndTimeSpan.ToString(@"hh\:mm"),
                        WillReturn = ReturnToday ? "Yes" : "No",
                        ReasonType = ReasonType,


                    };
                    var bodyParameter = new DaypassRequestBParameters()
                    {
                        Reason = Reason
                    };
                    var response = await _dataService.NewDayPassAsync(qParamters, bodyParameter);

                    if (response.ResponseStatus == ResponseStatus.SuccessWithResult)
                    {
                        if (!string.IsNullOrEmpty(response.Result.Message))
                        {
                            await _dialogManager.DisplayAlert(AppResources.ApplicationName, response.Result.Message, AppResources.Ok);
                        }
                    }
                    else
                    {
                        string message = _messageResolver.GetMessage(response);
                        await _dialogManager.DisplayAlert(AppResources.ApplicationName, message, AppResources.Ok);
                    }
                }
            }
            catch (Exception ex)
            {
                //Show error
#pragma warning disable 4014
                _dialogManager.DisplayAlert(AppResources.ApplicationName, AppResources.SomethingErrorHappened, AppResources.Ok);
#pragma warning restore 4014
            }
            finally
            {
                NewDayPassCommand.CanExecute = true;
                IsBusy = false;
            }
        }


        public async Task<ResponseWrapper<UserProfile>> LoadProfileAsync(string empId)
        {
            //Load other profile
            var parameters = new OtherProfileQParameters()
            {
                Langid = _appSettings.Language,
                UserToken = _appSettings.UserToken,
                EmpId = empId
            };

            return await _dataService.GetOtherUserProfile(parameters);
        }
        #endregion
    }
}
