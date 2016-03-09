using AdaaMobile.Common;
using AdaaMobile.DataServices;
using AdaaMobile.Helpers;
using AdaaMobile.Strings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaaMobile.ViewModels
{
    public class RequestDriverViewModel : BindableBase
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

		private string _Reason = "";
		public string Reason
		{
			get { return _Reason; }
			set { SetProperty(ref _Reason, value); }
		}

		private string _AdditionalComments = "";
		public string AdditionalComments
		{
			get { return _AdditionalComments; }
			set { SetProperty(ref _AdditionalComments, value); }
		}

        #endregion

        #region Initialization
        public RequestDriverViewModel(IDataService dataService, IDialogManager dialogManager, IAppSettings appSettings, INavigationService navigationService, IRequestMessageResolver messageResolver)
        {
            _dataService = dataService;
            _appSettings = appSettings;
            _navigationService = navigationService;
            _dialogManager = dialogManager;
            _messageResolver = messageResolver;
            NewDriverRequestCommand = new AsyncExtendedCommand(SubmitRequestAsync);

        }


        #endregion

        #region Commands
        public AsyncExtendedCommand NewDriverRequestCommand { get; set; }
        #endregion

        #region Methods

        private async Task SubmitRequestAsync()
        {
            try
            {
                NewDriverRequestCommand.CanExecute = false;
                IsBusy = true;
                BusyMessage = AppResources.Loading;
                //if (string.IsNullOrEmpty(Reason))
                //{
                //    await _dialogManager.DisplayAlert(AppResources.ApplicationName, AppResources.PleaseEnterReason, AppResources.Ok);
                //}
                //else
                //{
                //    var qParamters = new DaypassRequestQParameters()
                //    {
                //        Langid = _appSettings.Language,
                //        UserToken = _appSettings.UserToken,
                //        StartTime = StartTimeSpan.ToString(@"hh\:mm"),
                //        EndTime = EndTimeSpan.ToString(@"hh\:mm"),
                //        WillReturn = ReturnToday ? "Yes" : "No",
                //        ReasonType = ReasonType,

                //    };
                //    var bodyParameter = new DaypassRequestBParameters()
                //    {
                //        Reason = Reason
                //    };
                //    var response = await _dataService.NewDayPassAsync(qParamters, bodyParameter);

                //    if (response.ResponseStatus == ResponseStatus.SuccessWithResult)
                //    {
                //        string message = response.Result.Message;
                //        if (!string.IsNullOrEmpty(message))
                //        {
                //            string lowerCaseMessage = message.ToLower();
                //            await _dialogManager.DisplayAlert(AppResources.ApplicationName, message, AppResources.Ok);
                //            if (lowerCaseMessage.Contains(@"success")
                //                || lowerCaseMessage.Contains(@"completed")
                //                || lowerCaseMessage.Contains(@"نجاح"))
                //                _navigationService.GoBack();
                //        }
                //    }
                //    else
                //    {
                //        string message = _messageResolver.GetMessage(response);
                //        await _dialogManager.DisplayAlert(AppResources.ApplicationName, message, AppResources.Ok);
                //    }
                //}
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
                NewDriverRequestCommand.CanExecute = true;
                IsBusy = false;
            }
        }

        #endregion
    }
}
