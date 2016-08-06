using AdaaMobile.Helpers;
using System;
using AdaaMobile.Common;
using AdaaMobile.Models;
using AdaaMobile.Strings;
using System.Collections.ObjectModel;
using AdaaMobile.Views;

namespace AdaaMobile.ViewModels
{
	public class TimeSheetViewModel
	{
		#region Fields

		private readonly INavigationService _navigationService;
	
		#endregion

		#region Properties

		public ObservableCollection<AdaaPageItem> PagesList { get; private set; }

		#endregion

		#region Initialization

		public TimeSheetViewModel (INavigationService navigationService)
		{
			_navigationService = navigationService;
			PagesList = new ObservableCollection<AdaaPageItem> () {
				new AdaaPageItem () { TargetType = typeof(MyTimesheetPage), Title = AppResources.TimeSheet_MyTimeSheet },
				new AdaaPageItem () {TargetType = typeof(MyAssigmnetsPage),Title = AppResources.TimeSheet_MyAssignment},
				new AdaaPageItem (){ TargetType = typeof(MyPendingTasks), Title = AppResources.TimeSheet_MyPendingTasks }
			};
			NavigateToPageCommand = new ExtendedCommand<Type> (NavigateToPage);
		}

		#endregion

		#region Commands

		public ExtendedCommand<Type> NavigateToPageCommand { get; set; }

		#endregion

		#region Methods

		private void NavigateToPage (Type pageType)
		{
			_navigationService.NavigateToPage (pageType);
		}

		#endregion

	}
}
