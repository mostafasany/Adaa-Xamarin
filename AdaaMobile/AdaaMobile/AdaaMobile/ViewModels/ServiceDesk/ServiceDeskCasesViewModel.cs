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
	public class ServiceDeskCasesViewModel : BindableBase
	{
		#region Fields

		private readonly INavigationService _navigationService;
		private readonly IDataService _dataService;

		#endregion

		#region Properties

		private ServiceDeskCases _OriginalCasses;

		private ServiceDeskCases _Casses;

		public ServiceDeskCases Casses {
			get { return _Casses; }
			set { SetProperty (ref _Casses, value); OnPropertyChanged ("NoCasses");}
		}

		private ServiceDeskCase _SelectedCasses;

		public ServiceDeskCase SelectedCasses {
			get { return _SelectedCasses; }
			set { SetProperty (ref _SelectedCasses, value); }
		}

		public bool NoCasses {
			get 
			{
				if (Casses != null) {
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


		#endregion

		#region Initialization

		public ServiceDeskCasesViewModel (INavigationService navigationService, IDataService dataservice)
		{
			_navigationService = navigationService;
			_dataService = dataservice;
			PageLoadedCommand = new AsyncExtendedCommand(Loaded);
			CaseAccepted = new AsyncExtendedCommand(Accepted);
			CaseRejected = new AsyncExtendedCommand(Rejected);
			RequestItemSelectedCommand = new AsyncExtendedCommand<ServiceDeskCase> (OpenRequestDetailsPage);
		}


		#endregion

		#region Commands

		public AsyncExtendedCommand PageLoadedCommand { get; set; }
		public AsyncExtendedCommand CaseAccepted { get; set; }
		public AsyncExtendedCommand CaseRejected { get; set; }
		public AsyncExtendedCommand<ServiceDeskCase> RequestItemSelectedCommand { get; set; }

		#endregion

		#region Methods

		private async Task Loaded ()
		{
			try {
				IsBusy = true;
				var response = await _dataService.GetServiceDeskCases (null);
				if (response.ResponseStatus == ResponseStatus.SuccessWithResult && response.Result != null) {
					if (response.Result != null ) {
						Casses = response.Result;
						_OriginalCasses =response.Result;
					} else {

					}
				}

			} catch (Exception ex) {

			} finally {
				IsBusy = false;
			}

		}
		private async Task Accepted()
		{
			try
			{
				IsBusy = true;
				var response = await _dataService.AcceptServiceDeskCases(null);
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
		private async Task Rejected()
		{
			try
			{
				IsBusy = true;
				var response = await _dataService.RejectServiceDeskCases(null);
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
		private async Task OpenRequestDetailsPage (ServiceDeskCase casses)
		{
			SelectedCasses = casses;
			await LoadDetails();
			_navigationService.NavigateToPage (typeof(SelectedServiceCasesPage));
		}

		private async Task LoadDetails()
		{
			try
			{
				IsBusy = true;
				var response = await _dataService.GetServiceDeskCasesDetails(SelectedCasses.Parent_ID,null);
				if (response.ResponseStatus == ResponseStatus.SuccessWithResult && response.Result != null)
				{
					if (response.Result != null)
					{
						
						_SelectedCasses = response.Result.result.Table1[0];
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
		#endregion

	}
}
