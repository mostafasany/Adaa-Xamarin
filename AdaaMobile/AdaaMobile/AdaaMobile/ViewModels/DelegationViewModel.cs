using AdaaMobile.Common;
using AdaaMobile.DataServices;
using AdaaMobile.DataServices.Requests;
using AdaaMobile.Helpers;
using AdaaMobile.Models;
using AdaaMobile.Models.Request;
using AdaaMobile.Strings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.Models.Response;
using AdaaMobile.Views.Delegation;

namespace AdaaMobile.ViewModels
{
	public class DelegationViewModel : BindableBase
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



		private bool _showNoDelegations;

		public bool ShowNoDelegations {
			get { return _showNoDelegations; }
			set { SetProperty (ref _showNoDelegations, value); }
		}

		private Delegation _selectedDelegation;

		public Delegation SelectedDelegation {
			get { return _selectedDelegation; }
			set { SetProperty (ref _selectedDelegation, value); }
		}

		private ObservableCollection<Delegation> _delegations;

		public ObservableCollection<Delegation> Delegations {
			get { return _delegations; }
			set { SetProperty (ref _delegations, value); }
		}

		#endregion

		#region Initialization

		public DelegationViewModel (IDataService dataService, IDialogManager dialogManager, IAppSettings appSettings, INavigationService navigationService, IRequestMessageResolver messageResolver)
		{
			_dataService = dataService;
			_appSettings = appSettings;
			_navigationService = navigationService;
			_dialogManager = dialogManager;
			_messageResolver = messageResolver;
			LoadDayPassDataCommand = new AsyncExtendedCommand (LoadDelagationDataAsync);
			NavigateToPageCommand = new ExtendedCommand<object> (NavigateToPage);

		}

		#endregion

		#region Commands

		public AsyncExtendedCommand LoadDayPassDataCommand { get; set; }

		public ExtendedCommand<object> NavigateToPageCommand { get; set; }

		#endregion

		#region Methods

		private void NavigateToPage (object obj)
		{
			Models.Response.Delegation delegation = (obj as Models.Response.Delegation);
			if (delegation != null) {
				SelectedDelegation = delegation;
				_navigationService.NavigateToPage (typeof(DelegationDetailsPage));

			}
		}

		private async Task LoadDelagationDataAsync ()
		{
			try {
				IsBusy = true;
				LoadDayPassDataCommand.CanExecute = false;
				ShowNoDelegations = false;
				var response = await _dataService.GetAllDelegationsResponseAsync (new DelegationsQParamters () {
					Langid = _appSettings.Language,
					UserToken = _appSettings.UserToken
				});

				if (response.ResponseStatus == ResponseStatus.SuccessWithResult && response.Result != null) {
					if (response.Result.Delegations != null && response.Result.Delegations.Length > 0) {
						Delegations = new ObservableCollection<Delegation> (response.Result.Delegations);
						ShowNoDelegations = false;
					} else {
						Delegations = null;
						ShowNoDelegations = true;
					}

				}
			} catch (Exception) {
				// ignored
			} finally {
				LoadDayPassDataCommand.CanExecute = true;
				IsBusy = false;
			}
		}


       
		#endregion
	}
}
