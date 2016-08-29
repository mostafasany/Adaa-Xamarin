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
	public class MyAssigmentsViewModel : BindableBase
	{
		#region Fields

		private readonly INavigationService _navigationService;
		private readonly IDataService _dataService;

		#endregion

		#region Properties

		private List<Assignment> _OriginalAssignments;

		private List<Assignment> _Assignment;

		public List<Assignment> Assignment {
			get { return _Assignment; }
			set { SetProperty (ref _Assignment, value); OnPropertyChanged ("NoAssignments");}
		}

		private Assignment _SelectedAssignment;

		public Assignment SelectedAssignment {
			get { return _SelectedAssignment; }
			set { SetProperty (ref _SelectedAssignment, value); }
		}

		public bool NoAssignments {
			get 
			{
				if (Assignment != null && Assignment.Count > 0) {
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

		public MyAssigmentsViewModel (INavigationService navigationService, IDataService dataservice)
		{
			_navigationService = navigationService;
			_dataService = dataservice;
			PageLoadedCommand = new AsyncExtendedCommand (Loaded);
			RequestItemSelectedCommand = new AsyncExtendedCommand<Assignment> (OpenRequestDetailsPage);
		}

		public void FilterByDate(string year)
		{
			if(_OriginalAssignments!=null)
			Assignment = _OriginalAssignments.Where(a => a.Year == year).ToList();
		}

		#endregion

		#region Commands

		public AsyncExtendedCommand PageLoadedCommand { get; set; }

		public AsyncExtendedCommand<Assignment> RequestItemSelectedCommand { get; set; }

		#endregion

		#region Methods

		private async Task Loaded ()
		{
			try {
				IsBusy = true;
				var response = await _dataService.GetAssignmentAsync (null);
				if (response.ResponseStatus == ResponseStatus.SuccessWithResult && response.Result != null) {
					if (response.Result != null && response.Result.Count > 0) {
						Assignment = response.Result;
						_OriginalAssignments=response.Result;
					} else {

					}
				}

			} catch (Exception ex) {

			} finally {
				IsBusy = false;
			}

		}

		private async Task OpenRequestDetailsPage (Assignment assigmnet)
		{
			SelectedAssignment = assigmnet;
			_navigationService.NavigateToPage (typeof(SelectedAssimentsPage));
		}

		#endregion

	}
}
