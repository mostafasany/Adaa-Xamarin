using AdaaMobile.DataServices;
using AdaaMobile.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdaaMobile.Common;
using AdaaMobile.Models.Response;
using AdaaMobile.DataServices.Requests;
using System.Linq;

namespace AdaaMobile.ViewModels
{
	public class ServiceDeskRequestsViewModel : BindableBase
	{
		#region Fields

		private readonly INavigationService _navigationService;
		private readonly IDataService _dataService;

		#endregion

		#region Properties
		private List<ServiceDeskRequests> _IncidentRequests;
		private List<ServiceDeskRequests> _ServiceRequests;

		private List<ServiceDeskRequests> _OriginalRequests;

		private List<ServiceDeskRequests> _Requests;

		public List<ServiceDeskRequests> Requests {
			get { return _Requests; }
			set { SetProperty (ref _Requests, value); OnPropertyChanged ("NoRequests");}
		}

		private ServiceDeskRequests _SelectedRequests;

		public ServiceDeskRequests SelectedRequests {
			get { return _SelectedRequests; }
			set { SetProperty (ref _SelectedRequests, value); }
		}

		public bool NoRequests {
			get 
			{
				if (Requests != null && Requests.Count > 0) {
					return false;
				} else {
					return true;
				}
			}
		}

		private bool _IsBusy;

		public bool IsBusy {
			get { return _IsBusy; }
			set {
				SetProperty (ref _IsBusy, value);
			}
		}

		public bool incidents
		{
			get;
			set;

		}


		#endregion

		#region Initialization

		public ServiceDeskRequestsViewModel (INavigationService navigationService, IDataService dataservice)
		{
			_navigationService = navigationService;
			_dataService = dataservice;
			PageLoadedCommand = new AsyncExtendedCommand (Loaded);
			RequestItemSelectedCommand = new AsyncExtendedCommand<ServiceDeskRequests> (OpenRequestDetailsPage);
		}


		#endregion

		#region Commands

		public AsyncExtendedCommand PageLoadedCommand { get; set; }

		public AsyncExtendedCommand<ServiceDeskRequests> RequestItemSelectedCommand { get; set; }

		#endregion

		#region Methods

		private async Task Loaded ()
		{
			try {
				IsBusy = true;
				var response = await _dataService.GetServiceDeskRequests (incidents,null);
				if (response.ResponseStatus == ResponseStatus.SuccessWithResult && response.Result != null) {
					if (response.Result != null && response.Result.Count > 0) {

						_ServiceRequests = response.Result;
			
					} else {

					}
				}
				incidents = true;
				await LoadIncidints();
			} catch (Exception ex) {

			} finally {
				IsBusy = false;
			}

		}

		private async Task LoadIncidints()
		{
			try
			{
				IsBusy = true;
				var response = await _dataService.GetServiceDeskRequests(incidents, null);
				if (response.ResponseStatus == ResponseStatus.SuccessWithResult && response.Result != null)
				{
					if (response.Result != null && response.Result.Count > 0)
					{
						

							_IncidentRequests = response.Result;
					
						if (_ServiceRequests != null)
						{
							Requests = _IncidentRequests.Concat(_ServiceRequests).ToList();

						}
						else
						{
							Requests = _IncidentRequests;
						}
					}
					else {

					}
				}

			}
			catch (Exception ex)
			{

			}
			finally
			{
				IsBusy = false;
			}

		}

		private async Task OpenRequestDetailsPage (ServiceDeskRequests requests)
		{
			SelectedRequests = requests;
			//_navigationService.NavigateToPage (typeof(SelectedAssimentsPage));
		}

		#endregion

	}
}
