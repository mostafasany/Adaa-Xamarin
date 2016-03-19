using AdaaMobile.Common;
using AdaaMobile.DataServices;
using AdaaMobile.DataServices.Requests;
using AdaaMobile.Helpers;
using AdaaMobile.Models.Request;
using AdaaMobile.Models.Response;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaaMobile.ViewModels
{
    public class MyRequestsViewModel : BindableBase
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

		private Request _SelectedRequest;
		public Request SelectedRequest
		{
			get { return _SelectedRequest; }
			set { SetProperty(ref _SelectedRequest, value); }
		}


        private ObservableCollection<Request> _pendingRequests;
        public ObservableCollection<Request> PendingRequests
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
        #endregion

        #region Initialization

        public MyRequestsViewModel(IDataService dataService, IDialogManager dialogManager, 
			IAppSettings appSettings, INavigationService navigationService, IRequestMessageResolver messageResolver)
        {
			_navigationService = navigationService;
            _dataService = dataService;
            _appSettings = appSettings;
            _dialogManager = dialogManager;
            _messageResolver = messageResolver;
            LoadDayPassDataCommand = new AsyncExtendedCommand(LoadDayPassDataAsync);
			RequestItemSelectedCommand = new AsyncExtendedCommand<Request> (OpenRequestDetailsPage);
        }
        #endregion

        #region Commands
		public AsyncExtendedCommand LoadDayPassDataCommand { get; set; }
		public AsyncExtendedCommand<Request> RequestItemSelectedCommand { get; set; }

        #endregion

        #region Methods
        private async Task LoadDayPassDataAsync()
        {
            //Pending requests
            try

            {
				IsBusy = true;
                var response = await _dataService.GetAllSharepointRequestsAsync(new GetAllRequestsQParameters()
                {
                    Langid = _appSettings.Language,
                    UserToken = _appSettings.UserToken
                });

                if (response.ResponseStatus == ResponseStatus.SuccessWithResult && response.Result != null)
                {
                    if (response.Result.items != null && response.Result.items.Length > 0)
                    {
                        PendingRequests = new ObservableCollection<Request>(response.Result.items);
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


        }
		private async Task OpenRequestDetailsPage( Request selectedRequest)
		{
			SelectedRequest = selectedRequest;
			_navigationService.NavigateToPage (typeof(SelectedRequestPage));
		}

        #endregion
    }
}
