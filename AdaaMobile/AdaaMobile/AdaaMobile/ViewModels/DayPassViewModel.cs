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


        private ObservableCollection<DayPassRequest> _PendingRequests;
        public ObservableCollection<DayPassRequest> PendingRequests
        {
            get { return _PendingRequests; }
            set { SetProperty(ref _PendingRequests, value); }
        }


        private bool _ShowNoPendingRequests;
        public bool ShowNoPendingRequests
        {
            get { return _ShowNoPendingRequests; }
            set { SetProperty(ref _ShowNoPendingRequests, value); }
        }

        private ObservableCollection<DayPassTask> _DayPassTasks;
        public ObservableCollection<DayPassTask> DayPassTasks
        {
            get { return _DayPassTasks; }
            set { SetProperty(ref _DayPassTasks, value); }
        }


        private bool _ShowNoTasks;
        public bool ShowNoTasks
        {
            get { return _ShowNoTasks; }
            set { SetProperty(ref _ShowNoTasks, value); }
        }

        private bool _ReturnToday;
        public bool ReturnToday
        {
            get { return _ReturnToday; }
            set { SetProperty(ref _ReturnToday, value); }
        }


        private string _Reason;
        public string Reason
        {
            get { return _Reason; }
            set { SetProperty(ref _Reason, value); }
        }


        private string _StartTime;
        public string StartTime
        {
            get { return _StartTime; }
            set { SetProperty(ref _StartTime, value); }
        }



        private TimeSpan _EndTimeSpan;
        public TimeSpan EndTimeSpan
        {
            get { return _EndTimeSpan; }
            set { SetProperty(ref _EndTimeSpan, value); }
        }

        private string _EndTime;
        public string EndTime
        {
            get { return _EndTime; }
            set { SetProperty(ref _EndTime, value); }
        }


        private string _ReasonType = "Work";
        public string ReasonType
        {
            get { return _ReasonType; }
            set { SetProperty(ref _ReasonType, value); }
        }



        private DayPassRequest _SelectedRequest;
        public DayPassRequest SelectedRequest
        {
            get { return _SelectedRequest; }
            set { SetProperty(ref _SelectedRequest, value); }
        }


        private DayPassTask _SelectedTask;
        public DayPassTask SelectedTask
        {
            get { return _SelectedTask; }
            set { SetProperty(ref _SelectedTask, value); }
        }


        private TimeSpan _StartTimeSpan;
        public TimeSpan StartTimeSpan
        {
            get { return _StartTimeSpan; }
            set { SetProperty(ref _StartTimeSpan, value); }
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
                    //await _dialogManager.DisplayAlert(AppResources.ApplicationName, AppResources.PleaseenterUserName, AppResources.Ok);
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
                            await _dialogManager.DisplayAlert(AppResources.ApplicationName, AppResources.EnterValidPassword, AppResources.Ok);
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
