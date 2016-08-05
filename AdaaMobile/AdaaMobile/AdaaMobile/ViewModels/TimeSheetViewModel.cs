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

namespace AdaaMobile.ViewModels
{
	public class EServicesViewModel
	{
		#region Fields

		private readonly INavigationService _navigationService;
		private readonly IDataService _dataService;
		private readonly IAppSettings _appSettings;
		private readonly IRequestMessageResolver _messageResolver;
		private readonly IDialogManager _dialogManager;

		#endregion

		#region Properties

		public ObservableCollection<AdaaPageItem> PagesList { get; private set; }

		#endregion

		#region Initialization

		public EServicesViewModel (IDataService dataService, IAppSettings appSettings, INavigationService navigationService, IRequestMessageResolver messageResolver, IDialogManager dialogManager)
		{
			_dataService = dataService;
			_appSettings = appSettings;
			_navigationService = navigationService;
			_messageResolver = messageResolver;
			_dialogManager = dialogManager;
			PagesList = new ObservableCollection<AdaaPageItem> () {
				// new AdaaPageItem() {TargetType = typeof(RequestAnnouncementPage),Title = AppResources.RequestAnnouncement},
				// new AdaaPageItem() {TargetType = typeof(RequestAnnouncementPage),Title = AppResources.RequestAnnouncement},
				new AdaaPageItem () { TargetType = typeof(RequestDriverPage), Title = AppResources.RequestDriver },
				new AdaaPageItem () {
					TargetType = typeof(RequestOfficeMaintenancePage),
					Title = AppResources.RequestOfficeMaintenance
				},
				// new AdaaPageItem() {TargetType = typeof(RequestOfficeServicesPage),Title = AppResources.RequestOfficeService},
				new AdaaPageItem (){ TargetType = typeof(GreetingCardsPage), Title = AppResources.GreetingCard }

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
