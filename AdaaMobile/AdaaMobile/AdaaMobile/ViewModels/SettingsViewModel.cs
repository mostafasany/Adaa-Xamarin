using AdaaMobile.Common;
using AdaaMobile.DataServices;
using AdaaMobile.Helpers;
using AdaaMobile.Strings;
using System;
using System.Threading.Tasks;

namespace AdaaMobile
{
    public class SettingsViewModel : BindableBase
    {
        #region Fields
        private string _otherUserId;
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



        #endregion

        #region Initialization
        public SettingsViewModel(IDataService dataService, IAppSettings appSettings, IDialogManager dialogManager, IRequestMessageResolver messageResolver)
        {
            _dataService = dataService;
            _appSettings = appSettings;
            _dialogManager = dialogManager;
            _messageResolver = messageResolver;

            LogoutCommand = new AsyncExtendedCommand(DoLogout);

        }



        #endregion

        #region Commands

        public AsyncExtendedCommand LogoutCommand { get; set; }

        #endregion

        #region Methods
        private async Task DoLogout()
        {
            _appSettings.UserToken = string.Empty;

        }

        #endregion

    }

}
