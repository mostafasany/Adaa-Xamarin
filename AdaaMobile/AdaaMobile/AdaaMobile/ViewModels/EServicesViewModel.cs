using AdaaMobile.DataServices;
using AdaaMobile.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.Common;

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
        #endregion

        #region Initialization
        public EServicesViewModel(IDataService dataService, IAppSettings appSettings, INavigationService navigationService, IRequestMessageResolver messageResolver, IDialogManager dialogManager)
        {
            _dataService = dataService;
            _appSettings = appSettings;
            _navigationService = navigationService;
            _messageResolver = messageResolver;
            _dialogManager = dialogManager;
			NavigateToRequestDriverCommand = new ExtendedCommand (DoNavigateToRequestDriverCommand);
			NavigateToRequestOfficeMaintenanceCommand = new ExtendedCommand (DoNavigateToRequestOfficeMaintenanceCommand);

        }



        #endregion

        #region Commands
		public ExtendedCommand NavigateToRequestDriverCommand { get; set; }
		public ExtendedCommand NavigateToRequestOfficeMaintenanceCommand { get; set; }
        #endregion

        #region Methods
		private void DoNavigateToRequestDriverCommand()
		{
			_navigationService.NavigateToPage(typeof(RequestDriverPage));
		}
				private void DoNavigateToRequestOfficeMaintenanceCommand ()
		{
			_navigationService.NavigateToPage(typeof(RequestOfficeMaintenancePage));
		}
        #endregion

    }
}
