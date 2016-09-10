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

		private List<ServiceDeskCases> _OriginalCasses;

		private List<ServiceDeskCases> _Casses;

		public List<ServiceDeskCases> Casses {
			get { return _Casses; }
			set { SetProperty (ref _Casses, value); OnPropertyChanged ("NoCasses");}
		}

		private ServiceDeskCases _SelectedCasses;

		public ServiceDeskCases SelectedCasses {
			get { return _SelectedCasses; }
			set { SetProperty (ref _SelectedCasses, value); }
		}

		public bool NoCasses {
			get 
			{
				if (Casses != null && Casses.Count > 0) {
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
			PageLoadedCommand = new AsyncExtendedCommand (Loaded);
			RequestItemSelectedCommand = new AsyncExtendedCommand<ServiceDeskCases> (OpenRequestDetailsPage);
		}


		#endregion

		#region Commands

		public AsyncExtendedCommand PageLoadedCommand { get; set; }

		public AsyncExtendedCommand<ServiceDeskCases> RequestItemSelectedCommand { get; set; }

		#endregion

		#region Methods

		private async Task Loaded ()
		{
			try {
				IsBusy = true;
				var response = await _dataService.GetServiceDeskCases (null);
				if (response.ResponseStatus == ResponseStatus.SuccessWithResult && response.Result != null) {
					if (response.Result != null && response.Result.Count > 0) {
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

		private async Task OpenRequestDetailsPage (ServiceDeskCases casses)
		{
			SelectedCasses = casses;
			_navigationService.NavigateToPage (typeof(SelectedAssimentsPage));
		}

		#endregion

	}
}
