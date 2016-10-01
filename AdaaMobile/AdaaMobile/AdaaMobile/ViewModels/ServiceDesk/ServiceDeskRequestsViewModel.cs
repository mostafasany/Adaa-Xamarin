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
using System.Collections.ObjectModel;

namespace AdaaMobile.ViewModels
{
	public class ServiceDeskRequestsViewModel : BindableBase
	{
		#region Fields

		private readonly INavigationService _navigationService;
		private readonly IDataService _dataService;

		#endregion

		#region Properties
		private ObservableCollection<ServiceDeskRequest> _IncidentRequests;
		private ObservableCollection<ServiceDeskRequest> _ServiceRequests;

		private ObservableCollection<ServiceDeskRequest> _OriginalRequests;

		private ObservableCollection<ServiceDeskRequest> _Requests;

		public ObservableCollection<ServiceDeskRequest> Requests
		{
			get { return _Requests; }
			set { SetProperty(ref _Requests, value); OnPropertyChanged("NoRequests"); }
		}

		private ServiceDeskRequest _SelectedRequests;

		public ServiceDeskRequest SelectedRequests
		{
			get { return _SelectedRequests; }
			set { SetProperty(ref _SelectedRequests, value); }
		}

		public bool NoRequests
		{
			get
			{
				if (Requests != null)
				{
					return false;
				}
				else {
					return true;
				}
			}
		}

		private bool _IsBusy;

		public bool IsBusy
		{
			get { return _IsBusy; }
			set
			{
				SetProperty(ref _IsBusy, value);
			}
		}



		#endregion

		#region Initialization

		public ServiceDeskRequestsViewModel(INavigationService navigationService, IDataService dataservice)
		{
			_navigationService = navigationService;
			_dataService = dataservice;
			PageLoadedCommand = new AsyncExtendedCommand(Loaded);
			RequestItemSelectedCommand = new AsyncExtendedCommand<ServiceDeskRequest>(OpenRequestDetailsPage);
			CancelRequest = new AsyncExtendedCommand(Cancel);
		}

		public void FilterByStatus(string status)
		{
			if (_OriginalRequests != null)
			{
				if (status == "All")
				{
					Requests = _OriginalRequests;
				}
				else
				{
					List<ServiceDeskRequest> objlist = _OriginalRequests.ToList();
					List<ServiceDeskRequest> resultlist = objlist.Where(a => a.Status == status).ToList();
					Requests = new ObservableCollection<ServiceDeskRequest>(resultlist);

				}
			}
		}

		public void searchBy(string search)
		{
			if (_OriginalRequests != null)
			{
				if (string.IsNullOrEmpty(search))
				{
					Requests = _OriginalRequests;
				}
				else
				{
					try
					{
						List<ServiceDeskRequest> objlist = _OriginalRequests.ToList();
						List<ServiceDeskRequest> resultlist = objlist.Where(a => a.Classification.ToLower().Contains(search.ToLower()) ||
						                                                    a.Title.ToLower().Contains(search.ToLower())).ToList();
						Requests = new ObservableCollection<ServiceDeskRequest>(resultlist);
					}
					catch (Exception ex)
					{

					}
	

				}
			}
		}

		#endregion

		#region Commands

		public AsyncExtendedCommand PageLoadedCommand { get; set; }
		public AsyncExtendedCommand CancelRequest { get; set; }
		public AsyncExtendedCommand<ServiceDeskRequest> RequestItemSelectedCommand { get; set; }

		#endregion

		#region Methods

		private async Task Loaded()
		{
			try
			{
				IsBusy = true;
				var response = await _dataService.GetServiceDeskRequests(false, null);
				if (response.ResponseStatus == ResponseStatus.SuccessWithResult && response.Result != null)
				{
					if (response.Result != null)
					{

						_ServiceRequests =new ObservableCollection<ServiceDeskRequest>(response.Result.result.Table1);  

					}
					else {

					}
				}

				await LoadIncidints();
			}
			catch (Exception ex)
			{

			}
			finally
			{
				IsBusy = false;
			}

		}

		private async Task LoadIncidints()
		{
			try
			{
				IsBusy = true;
				var response = await _dataService.GetServiceDeskRequests(true, null);
				if (response.ResponseStatus == ResponseStatus.SuccessWithResult && response.Result != null)
				{
					if (response.Result != null)
					{


						_IncidentRequests = new ObservableCollection<ServiceDeskRequest>(response.Result.result.Table1);

						if (_ServiceRequests != null)
						{
							
							List<ServiceDeskRequest> list = new List<ServiceDeskRequest>();
							list.AddRange(_IncidentRequests);
							list.AddRange(_ServiceRequests);
							Requests = new ObservableCollection<ServiceDeskRequest>(list);


						}
						else
						{
							Requests = _IncidentRequests;
						}
						_OriginalRequests = new ObservableCollection<ServiceDeskRequest>(Requests.ToList());
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
						if (response.Result.result == "")
						{
							Locator.Default.NavigationService.GoBack();

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

		private async Task OpenRequestDetailsPage(ServiceDeskRequest request)
		{
			SelectedRequests = request;
			_navigationService.NavigateToPage(typeof(SelectedServiceRequestPage));
		}

		#endregion

	}
}
