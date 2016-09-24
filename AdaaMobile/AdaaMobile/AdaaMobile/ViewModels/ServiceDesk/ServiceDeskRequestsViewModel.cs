using AdaaMobile.DataServices;
using AdaaMobile.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdaaMobile.Common;
using AdaaMobile.Models.Response;
using AdaaMobile.DataServices.Requests;
using System.Linq;
using System.Diagnostics.Contracts;

namespace AdaaMobile.ViewModels
{
	public class ServiceDeskRequestsViewModel : BindableBase
	{
		#region Fields

		private readonly INavigationService _navigationService;
		private readonly IDataService _dataService;

		#endregion

		#region Properties
		private ServiceDeskRequests _IncidentRequests;
		private ServiceDeskRequests _ServiceRequests;

		private ServiceDeskRequests _OriginalRequests;

		private ServiceDeskRequests _Requests;

		public ServiceDeskRequests Requests {
			get { return _Requests; }
			set { SetProperty (ref _Requests, value); OnPropertyChanged ("NoRequests");}
		}

		private ServiceDeskRequest _SelectedRequests;

		public ServiceDeskRequest SelectedRequests {
			get { return _SelectedRequests; }
			set { SetProperty (ref _SelectedRequests, value); }
		}

		public bool NoRequests {
			get 
			{
				if (Requests != null )
				{
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
			RequestItemSelectedCommand = new AsyncExtendedCommand<ServiceDeskRequest> (OpenRequestDetailsPage);
			CancelRequest = new AsyncExtendedCommand(Cancel);
		}


		#endregion

		#region Commands

		public AsyncExtendedCommand PageLoadedCommand { get; set; }
		public AsyncExtendedCommand CancelRequest { get; set; }
		public AsyncExtendedCommand<ServiceDeskRequest> RequestItemSelectedCommand { get; set; }

		#endregion

		#region Methods

		private async Task Loaded ()
		{
			try {
				IsBusy = true;
				var response = await _dataService.GetServiceDeskRequests (incidents,null);
				if (response.ResponseStatus == ResponseStatus.SuccessWithResult && response.Result != null) {
					if (response.Result != null ) {

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
					if (response.Result != null)
					{
						

						_IncidentRequests = response.Result;
					
						if (_ServiceRequests != null)
						{
							List<ServiceDeskRequest> list = new List<ServiceDeskRequest>();
							list.AddRange(_IncidentRequests.result.Table1);
							list.AddRange(_ServiceRequests.result.Table1);
							Requests.result.Table1 = list.ToArray();

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

		private async Task Cancel()
		{
			try
			{
				IsBusy = true;
				var response = await _dataService.CancelServiceDeskRequests(SelectedRequests, null);
				if (response.ResponseStatus == ResponseStatus.SuccessWithResult && response.Result != null)
				{
					if (response.Result != null)
					{
					
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

		private async Task OpenRequestDetailsPage (ServiceDeskRequest request)
		{
			SelectedRequests = request;
			_navigationService.NavigateToPage (typeof(SelectedServiceRequestPage));
		}

		#endregion

	}
}
