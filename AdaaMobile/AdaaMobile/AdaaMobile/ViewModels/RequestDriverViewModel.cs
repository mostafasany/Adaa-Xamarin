using AdaaMobile.Common;
using AdaaMobile.DataServices;
using AdaaMobile.DataServices.Requests;
using AdaaMobile.Helpers;
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

        private string _SelectedPiorityText = AppResources.Normal;
        public string SelectedPiorityText
        {
            get { return _SelectedPiorityText; }
            set { SetProperty(ref _SelectedPiorityText, value); }
        }



        private List<BaseItem> _ClientsList = new List<BaseItem>();
        public List<BaseItem> ClientsList
        {
            get { return _ClientsList; }
            set { SetProperty(ref _ClientsList, value); }
        }


        private string _SelectedDestinationName;
        public string SelectedDestinationName
        {
            get { return _SelectedDestinationName; }
            set { SetProperty(ref _SelectedDestinationName, value); }
        }

        private string _SelectedSourceName;
        public string SelectedSourceName
        {
            get { return _SelectedSourceName; }
            set { SetProperty(ref _SelectedSourceName, value); }
        }

        private DateTime _RequestDate;
        public DateTime RequestDate
        {
            get { return _RequestDate; }
            set { SetProperty(ref _RequestDate, value); }
        }

        private TimeSpan _RequestTimeSpan = new TimeSpan(8, 0, 0);
        public TimeSpan RequestTimeSpan
        {
            get { return _RequestTimeSpan; }
            set
            {
                DateTime d = DateTime.Today.Add(value);
                RequestTimeSpanString = d.ToString("hh:mm tt");
                SetProperty(ref _RequestTimeSpan, value);
            }
        }

        private string _RequestTimeSpanString;
        public string RequestTimeSpanString
        {
            get { return _RequestTimeSpanString; }
            set { SetProperty(ref _RequestTimeSpanString, value); }
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
            LoadCommand = new AsyncExtendedCommand(LoadAsync);
            RequestDate = DateTime.Now;
            RequestTimeSpan = new TimeSpan(8, 0, 0);
        }


        #endregion

        #region Commands
        public AsyncExtendedCommand NewDriverRequestCommand { get; set; }
        public AsyncExtendedCommand LoadCommand { get; set; }

        #endregion

        #region Methods

        public async Task GetClients()
        {
            try
            {
                LoadCommand.CanExecute = false;
                IsBusy = true;

                var paramters = new GetClientsQParameters()
                {
                    Langid = _appSettings.Language,
                    UserToken = _appSettings.UserToken
                };
                var result = await _dataService.GetClientsAsync(paramters);

				if (result.ResponseStatus == ResponseStatus.SuccessWithResult && result.Result != null && result.Result.item != null)
                {
                    ClientsList = new List<BaseItem>(result.Result.item);
                    if (ClientsList.Count > 0)
                    {
                        SelectedDestinationName = ClientsList[0].title;
                        SelectedSourceName = ClientsList[0].title;
                    }
                }
                else
                {
                    string message = _messageResolver.GetMessage(result);
                    await _dialogManager.DisplayAlert(AppResources.ApplicationName, message, AppResources.Ok);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                LoadCommand.CanExecute = true;
                IsBusy = false;
            }
        }
        private async Task LoadAsync()
        {
            try
            {
                LoadCommand.CanExecute = false;
                IsBusy = true;

                await GetClients();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                LoadCommand.CanExecute = true;
                IsBusy = false;
            }

        }
        private async Task SubmitRequestAsync()
        {
            try
            {
                NewDriverRequestCommand.CanExecute = false;
                IsBusy = true;
                BusyMessage = AppResources.Loading;
                if (string.IsNullOrEmpty(Reason))
                {
                    await _dialogManager.DisplayAlert(AppResources.ApplicationName, AppResources.PleaseEnterReason, AppResources.Ok);
                }
                else
                {
				var qParamters = new SaveDriverRequestQParameters()
                    {
                        Langid = _appSettings.Language,
                        UserToken = _appSettings.UserToken,


                    };

				var bodyParameter = new SaveDriverRequestBParameters()
                    {
					Destination = SelectedDestinationName,
						Priority = "1",//SelectedPiorityText
					Reason = Reason,
					Requestcomments = AdditionalComments,
					Requestdate = RequestDate.Add(RequestTimeSpan).ToString(@"dd/MM/yyyy hh:mm tt"),
					Requesttype = ReasonType,
					Source = SelectedSourceName
	
                    };
					var response = await _dataService.SaveDriverRequestAsync(qParamters, bodyParameter);

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
                NewDriverRequestCommand.CanExecute = true;
                IsBusy = false;
            }
        }

        #endregion
    }
}
