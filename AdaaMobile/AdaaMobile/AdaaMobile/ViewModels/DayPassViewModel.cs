using AdaaMobile.Common;
using AdaaMobile.DataServices;
using AdaaMobile.DataServices.Requests;
using AdaaMobile.Helpers;
using AdaaMobile.Models.Request;
using AdaaMobile.Models.Response;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AdaaMobile.Views.DayPass;

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

		public bool IsBusy {
			get { return _isBusy; }
			set { SetProperty (ref _isBusy, value); }
		}

		private string _busyMessage;

		public string BusyMessage {
			get { return _busyMessage; }
			set { SetProperty (ref _busyMessage, value); }
		}

		private ObservableCollection<DayPassRequest> _pendingRequests;

		public ObservableCollection<DayPassRequest> PendingRequests {
			get { return _pendingRequests; }
			set { SetProperty (ref _pendingRequests, value); }
		}


		private bool _showNoPendingRequests;

		public bool ShowNoPendingRequests {
			get { return _showNoPendingRequests; }
			set { SetProperty (ref _showNoPendingRequests, value); }
		}

		private ObservableCollection<DayPassTask> _dayPassTasks;

		public ObservableCollection<DayPassTask> DayPassTasks {
			get { return _dayPassTasks; }
			set { SetProperty (ref _dayPassTasks, value); }
		}

		private bool _showNoTasks;

		public bool ShowNoTasks {
			get { return _showNoTasks; }
			set { SetProperty (ref _showNoTasks, value); }
		}

		private DayPassRequest _selectedRequest;

		public DayPassRequest SelectedRequest {
			get { return _selectedRequest; }
			set { SetProperty (ref _selectedRequest, value); }
		}


		private DayPassTask _selectedTask;

		public DayPassTask SelectedTask {
			get { return _selectedTask; }
			set { SetProperty (ref _selectedTask, value); }
		}


		#endregion

		#region Initialization

		public DayPassViewModel (IDataService dataService, IDialogManager dialogManager, IAppSettings appSettings, INavigationService navigationService, IRequestMessageResolver messageResolver)
		{
			_dataService = dataService;
			_appSettings = appSettings;
			_navigationService = navigationService;
			_dialogManager = dialogManager;
			_messageResolver = messageResolver;
			LoadDayPassDataCommand = new AsyncExtendedCommand (LoadDayPassDataAsync);
			NavigateToRequestPageCommand = new ExtendedCommand<object> (DoNavigateToRequestDetailsPage);
		}

		#endregion

		#region Commands

		public AsyncExtendedCommand LoadDayPassDataCommand { get; set; }

		public ExtendedCommand<object> NavigateToRequestPageCommand { get; set; }

		#endregion

		#region Methods

		private void DoNavigateToRequestDetailsPage (object obj)
		{
			if (obj is DayPassTask) {
				
				DayPassTask task = (obj as DayPassTask);
				SelectedTask = task;
				_navigationService.NavigateToPage (typeof(DayPassTaskDetailsPage));
			} else {
				DayPassRequest request = (obj as DayPassRequest);
				SelectedRequest = request;
				_navigationService.NavigateToPage (typeof(DayPassRequestDetailsPage));
			}
		
		}

		private async Task LoadDayPassDataAsync ()
		{
			//Pending requests
			try {
				IsBusy = true;
				LoadDayPassDataCommand.CanExecute = false;
				ShowNoPendingRequests = false;
				var response = await _dataService.GetPendingDayPassesAsync (new DayPassesQParameters () {
					Langid = _appSettings.Language,
					UserToken = _appSettings.UserToken
				});

				if (response.ResponseStatus == ResponseStatus.SuccessWithResult && response.Result != null) {
					if (response.Result.DayPassList != null && response.Result.DayPassList.Length > 0) {
						PendingRequests = new ObservableCollection<DayPassRequest> (response.Result.DayPassList);
						ShowNoPendingRequests = false;
					} else {
						ShowNoPendingRequests = true;
					}
				}
			} catch (Exception) {
				// ignored
			} finally {
				LoadDayPassDataCommand.CanExecute = true;
				IsBusy = false;
			}

			//Get tasks
			try {
				IsBusy = true;
				LoadDayPassDataCommand.CanExecute = false;
				ShowNoTasks = false;
				var response = await _dataService.GetDayPassTasksResponseAsync (new DayPassTasksQParameters {
					Langid = _appSettings.Language,
					UserToken = _appSettings.UserToken
				});

				if (response.ResponseStatus == ResponseStatus.SuccessWithResult && response.Result != null) {
					if (response.Result.DayPassTaskList != null && response.Result.DayPassTaskList.Length > 0) {
						DayPassTasks = new ObservableCollection<DayPassTask> (response.Result.DayPassTaskList);
						ShowNoTasks = false;
					} else {
						DayPassTasks = null;
						ShowNoTasks = true;
					}

				}
			} catch (Exception) {
				// ignored
			} finally {
				LoadDayPassDataCommand.CanExecute = true;
				IsBusy = false;
			}
		}

		public async Task<ResponseWrapper<UserProfile>> LoadProfileAsync (string empId)
		{
			//Load other profile
			var parameters = new OtherProfileQParameters () {
				Langid = _appSettings.Language,
				UserToken = _appSettings.UserToken,
				EmpId = empId
			};

			return await _dataService.GetOtherUserProfile (parameters);
		}

		#endregion
	}
}
