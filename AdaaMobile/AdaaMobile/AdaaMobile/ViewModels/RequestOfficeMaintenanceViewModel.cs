using AdaaMobile.Common;
using AdaaMobile.DataServices;
using AdaaMobile.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaaMobile.ViewModels
{
    public class RequestOfficeMaintenanceViewModel : BindableBase
    {
        #region Fields
        private readonly INavigationService _navigationService;
        private readonly IDataService _dataService;
        private readonly IAppSettings _appSettings;
        private readonly IDialogManager _dialogManager;
        private readonly IRequestMessageResolver _messageResolver;
        #endregion

        #region Properties
        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }

        private string _busyMessage;
        public string BusyMessage
        {
            get { return _busyMessage; }
            set { SetProperty(ref _busyMessage, value); }
        }
        #endregion

        #region Initialization
        public RequestOfficeMaintenanceViewModel(IDataService dataService, IDialogManager dialogManager, IAppSettings appSettings, INavigationService navigationService, IRequestMessageResolver messageResolver)
        {
            _dataService = dataService;
            _appSettings = appSettings;
            _navigationService = navigationService;
            _dialogManager = dialogManager;
            _messageResolver = messageResolver;

        }


    }
    #endregion

    #region Commands
    #endregion

    #region Methods
    #endregion
}
}
