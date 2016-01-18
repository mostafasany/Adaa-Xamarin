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
    public class NewDayPassViewModel : BindableBase
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



		private TimeSpan _endTimeSpan = new TimeSpan(8,0,0);
        public TimeSpan EndTimeSpan
        {
            get { return _endTimeSpan; }
            set
            {
                DateTime d = DateTime.Today.Add(value);
                EndTimeSpanString = d.ToString("hh:mm tt");
                SetProperty(ref _endTimeSpan, value);
				CalculateDuration ();
            }
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

        private string _localizedReasonType = AppResources.Work;
        public string LocalizedReasonType
        {
            get { return _localizedReasonType; }
            set { SetProperty(ref _localizedReasonType, value); }
        }


		private TimeSpan _startTimeSpan = new TimeSpan(8,0,0);
        public TimeSpan StartTimeSpan
        {
            get { return _startTimeSpan; }
            set
            {
                DateTime d = DateTime.Today.Add(value);
                StartTimeSpanString = d.ToString("hh:mm tt");

                SetProperty(ref _startTimeSpan, value);
				CalculateDuration ();
            }
        }
        private string _startTimeSpanString;
        public string StartTimeSpanString
        {
            get { return _startTimeSpanString; }
            set { SetProperty(ref _startTimeSpanString, value); }
        }


        private string _DurationText = "---";
        public string DurationText
        {
            get
            {


                return _DurationText;
            }
            set
            {
                SetProperty(ref _DurationText, value);
            }
        }
        #endregion

        #region Initialization
        public NewDayPassViewModel(IDataService dataService, IDialogManager dialogManager, IAppSettings appSettings, INavigationService navigationService, IRequestMessageResolver messageResolver)
        {
            _dataService = dataService;
            _appSettings = appSettings;
            _navigationService = navigationService;
            _dialogManager = dialogManager;
            _messageResolver = messageResolver;
            NewDayPassCommand = new AsyncExtendedCommand(SubmitRequestAsync);
            StartTimeSpan = new TimeSpan(8, 0, 0);
            EndTimeSpan = new TimeSpan(8, 0, 0);
        }

        #endregion

        #region Commands
        public AsyncExtendedCommand NewDayPassCommand { get; set; }

        #endregion

        #region Methods

        private async Task SubmitRequestAsync()
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
                        string message = response.Result.Message;
                        if (!string.IsNullOrEmpty(message))
                        {
                            string lowerCaseMessage = message.ToLower();
                            await _dialogManager.DisplayAlert(AppResources.ApplicationName, message, AppResources.Ok);
                            if (lowerCaseMessage.Contains(@"success")
                                || lowerCaseMessage.Contains(@"completed")
                                || lowerCaseMessage.Contains(@"نجاح"))
                                _navigationService.GoBack();
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

        private void CalculateDuration()
        {
            try
            {
                if (StartTimeSpan != null && EndTimeSpan != null && EndTimeSpan >= StartTimeSpan)
                {
					//DateTime d = DateTime.Today.Add((EndTimeSpan - StartTimeSpan));
					//DurationText = d.ToString(@"hh\:mm");

					TimeSpan d = EndTimeSpan.Subtract(  StartTimeSpan);
					int totalHours = 0;
					int totalMinutes = 0;
					if(d.TotalMinutes >= 60){
						totalHours = (int)(d.TotalMinutes/60); 
						totalMinutes = (int)(d.TotalMinutes - (totalHours * 60));
					}
					else{
						totalMinutes = (int)(d.TotalMinutes);
					}
						
					DurationText = string.Format("{0} : {1}", totalHours, totalMinutes);
                }
                else
                {
                    DurationText = "---";
                }
            }
            catch (Exception ex)
            {

            }
            #endregion
        }
    }
    }
