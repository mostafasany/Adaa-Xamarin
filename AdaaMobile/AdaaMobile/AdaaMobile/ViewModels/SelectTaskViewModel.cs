using AdaaMobile.DataServices;
using AdaaMobile.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.Common;
using AdaaMobile.Models;
using AdaaMobile.Strings;
using AdaaMobile.Views.EServices;
using System.Collections.ObjectModel;
using AdaaMobile.Models.Response;
using AdaaMobile.DataServices.Requests;

namespace AdaaMobile.ViewModels
{
	public class SelectTaskViewModel: BindableBase
	{
		#region Fields

		private readonly INavigationService _navigationService;
		private readonly IDataService _dataService;

		#endregion

		#region Properties

		private List<PendingTask> _PendingTask;

		public List<PendingTask> PendingTask {
			get { return _PendingTask; }
			set { SetProperty (ref _PendingTask, value); }
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

		public SelectTaskViewModel (INavigationService navigationService, IDataService dataservice)
		{
			_navigationService = navigationService;
			_dataService = dataservice;
			PageLoadedCommand = new AsyncExtendedCommand (Loaded);
		}

		#endregion

		#region Commands

		public AsyncExtendedCommand PageLoadedCommand { get; set; }

		#endregion

		#region Methods

		private async Task Loaded ()
		{
			try {
				IsBusy = true;
				var response = await _dataService.GetPendingTaskAsync (null);
				if (response.ResponseStatus == ResponseStatus.SuccessWithResult && response.Result != null) {
					if (response.Result != null && response.Result.Count > 0) {
						PendingTask = response.Result;
					} else {

					}
				}

			} catch (Exception ex) {
				
			} finally {
				IsBusy = false;
			}

		}


		#endregion

	}
}
