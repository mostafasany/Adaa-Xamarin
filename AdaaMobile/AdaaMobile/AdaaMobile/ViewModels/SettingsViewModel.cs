using AdaaMobile.Common;
using AdaaMobile.DataServices;
using AdaaMobile.Helpers;
using AdaaMobile.Strings;
using System;
using System.Threading.Tasks;
using AdaaMobile.Views;

namespace AdaaMobile
{
    public class SettingsViewModel : BindableBase
    {
        #region Fields
        private readonly IDataService _dataService;
        private readonly IAppSettings _appSettings;
        private readonly IDialogManager _dialogManager;
        private readonly IRequestMessageResolver _messageResolver;
		private readonly INavigationService _navigationService;
        #endregion

        #region Properties




        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }

		private int _SelectedLanguageIndex;
		public int SelectedLanguageIndex
		{
			get { return _SelectedLanguageIndex; }
			set { 
				SetProperty(ref _SelectedLanguageIndex, value);
				UpdateLanguage (value);
			}
		}

        #endregion

        #region Initialization
		public SettingsViewModel(IDataService dataService, IAppSettings appSettings, IDialogManager dialogManager,
			IRequestMessageResolver messageResolver, INavigationService navigationService)
        {
            _dataService = dataService;
            _appSettings = appSettings;
            _dialogManager = dialogManager;
            _messageResolver = messageResolver;
			_navigationService = navigationService;

            LogoutCommand = new AsyncExtendedCommand(DoLogout);

			if (appSettings.SelectedCultureName == "en-us") {
				_SelectedLanguageIndex = 0;
			} else {
				_SelectedLanguageIndex = 1;
			}

        }



        #endregion

        #region Commands

		public AsyncExtendedCommand LogoutCommand { get; set; }

        #endregion

        #region Methods
        private async Task DoLogout()
        {
            _appSettings.UserToken = string.Empty;
			_navigationService.SetAppCurrentPage (typeof(LoginPage));

        }


		void UpdateLanguage (int value)
		{
			if (value == 0) {
				_appSettings.SelectedCultureName = "en-US";
			} else {
				_appSettings.SelectedCultureName = "ar-EG";
			}
		}
        #endregion

    }

}
