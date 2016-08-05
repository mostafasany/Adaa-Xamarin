using AdaaMobile.Common;
using AdaaMobile.DataServices;
using AdaaMobile.DataServices.Requests;
using AdaaMobile.Helpers;
using AdaaMobile.Models.Request;
using AdaaMobile.Strings;
using AdaaMobile.Views;
using System;
using System.Threading.Tasks;
using AdaaMobile.Models.Response;
using AdaaMobile.Views.Authentication;
using Xamarin.Forms;

namespace AdaaMobile.ViewModels
{
    public class UserAccountServicesViewModel : BindableBase
    {

        #region Fields

        private readonly INavigationService _navigationService;
        private readonly IDataService _dataService;
        private readonly IAppSettings _appSettings;
        private readonly IRequestMessageResolver _messageResolver;
        private readonly IDialogManager _dialogManager;

        private readonly Color _unResolvedColor = Color.Gray;
        private readonly Color _validColor = Color.Green;
        private readonly Color _inValidColor = Color.Red;
        #endregion

        #region Properties
        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }

        private string _accountStatus;
        public string AccountStatus
        {
            get { return _accountStatus; }
            set { SetProperty(ref _accountStatus, value); }
        }

		private bool _isAccountLocked;
		public bool IsAccountLocked
		{
			get { return _isAccountLocked; }
			set 
			{ 
				SetProperty(ref _isAccountLocked, value);


			}
		}
			

        private Color _accountColor;

        public Color AccountColor
        {
            get { return _accountColor; }
            set { SetProperty(ref _accountColor, value); }
        }

        private Color _passwordColor;

        public Color PasswordColor
        {
            get { return _passwordColor; }
            set { SetProperty(ref _passwordColor, value); }
        }


        private string _passwordStatus;
        public string PasswordStatus
        {
            get { return _passwordStatus; }
            set { SetProperty(ref _passwordStatus, value); }
        }
        #endregion

        #region Initialization

        public UserAccountServicesViewModel(IDataService dataService, IAppSettings appSettings, INavigationService navigationService, IRequestMessageResolver messageResolver, IDialogManager dialogManager)
        {
            _dataService = dataService;
            _appSettings = appSettings;
            _navigationService = navigationService;
            _messageResolver = messageResolver;
            _dialogManager = dialogManager;
            LoadCommand = new AsyncExtendedCommand(LoadAsync);
            UnlockMyAccountCommand = new AsyncExtendedCommand(UnlockMyAccountAsync);
            NavigateToChangePasswordCommand = new ExtendedCommand(DoNavigateToChangePassword);

            PasswordColor = _unResolvedColor;
            AccountColor = _unResolvedColor;
        }




        #endregion

        #region Commands
        public AsyncExtendedCommand LoadCommand { get; set; }
        public ExtendedCommand NavigateToChangePasswordCommand { get; set; }
        public AsyncExtendedCommand UnlockMyAccountCommand { get; set; }
        #endregion

        #region Methods

        private void SetPasswordColor(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) PasswordColor = _unResolvedColor;
            else if (message.Equals("Password Expired", StringComparison.OrdinalIgnoreCase)) PasswordColor = _inValidColor;
            else PasswordColor = _validColor;
        }

        private void SetAccountColor(string message)
        {
			if (string.IsNullOrWhiteSpace (message)) {
				AccountColor = _unResolvedColor;
				IsAccountLocked = false;
			} else if (message.Equals ("Account Locked", StringComparison.OrdinalIgnoreCase)) {
				AccountColor = _inValidColor;
				IsAccountLocked = true;
			} else {
				AccountColor = _validColor;
				IsAccountLocked = false;
			}
        }

        private void DoNavigateToChangePassword()
        {
			_navigationService.NavigateToPage(typeof(ChangePasswordPage));
        }

        private async Task UnlockMyAccountAsync()
        {
            try
            {
				if(!IsAccountLocked)
				{
					return;
				}
                UnlockMyAccountCommand.CanExecute = false;
                IsBusy = true;
                var parameters = new UnlockAccountQParameters()
                {
                    Langid = _appSettings.Language,
                    UserToken = _appSettings.UserToken
                };
                var response = await _dataService.UnlockAccountAsync(parameters);
                if (response.Result != null && !String.IsNullOrWhiteSpace(response.Result.Message))
                {
                    await _dialogManager.DisplayAlert(AppResources.ApplicationName, response.Result.Message, AppResources.Ok);
                }
                else
                {
                    await _dialogManager.DisplayAlert(AppResources.Error, _messageResolver.GetMessage(response), AppResources.Ok);
                    if (response.ResponseStatus == ResponseStatus.InvalidToken)
                    {
                        _navigationService.SetAppCurrentPage(typeof(LoginPage));
                    }
                }
            }
            catch (Exception)
            {
#pragma warning disable 4014
                _dialogManager.DisplayAlert(AppResources.Error, AppResources.UnlockAccountError, AppResources.Ok);
#pragma warning restore 4014
            }
            finally
            {
                UnlockMyAccountCommand.CanExecute = true;
                IsBusy = false;
            }

        }

        private async Task LoadAsync()
        {
            try
            {
                LoadCommand.CanExecute = false;
                IsBusy = true;
                var passwordResponse = await GetPasswordStatusAsync();
                if (passwordResponse.ResponseStatus == ResponseStatus.InvalidToken) return;
                var accountResponse = await GetAccountStatusAsync();
            }
            catch (Exception)
            {
                // ignored
            }
            finally
            {
                LoadCommand.CanExecute = true;
                IsBusy = false;
            }

        }

        private async Task<ResponseWrapper<PasswordStatusResponse>> GetPasswordStatusAsync()
        {
            var passwordparamters = new PasswordStatusQParameters()
            {
                Langid = _appSettings.Language,
                UserToken = _appSettings.UserToken
            };
            var passwordresult = await _dataService.GetPasswordStatusAsync(passwordparamters);

            if (passwordresult.ResponseStatus == ResponseStatus.SuccessWithResult && passwordresult.Result != null)
            {
                PasswordStatus = passwordresult.Result.Message;
                SetPasswordColor(passwordresult.Result.Message);

            }
            else
            {
                PasswordColor = _unResolvedColor;
                string message = _messageResolver.GetMessage(passwordresult);
                if (passwordresult.ResponseStatus == ResponseStatus.InvalidToken)
                {
                    await _dialogManager.DisplayAlert(AppResources.ApplicationName, message, AppResources.Ok);
                    _navigationService.SetAppCurrentPage(typeof(LoginPage));

                }
                else
                {
                    PasswordStatus = message;
                }
            }
            return passwordresult;
        }

        private async Task<ResponseWrapper<Models.Response.AccountStatusResponse>> GetAccountStatusAsync()
        {
            var paramters = new AccountStatusQParameters()
            {
                Langid = _appSettings.Language,
                UserToken = _appSettings.UserToken
            };
            var result = await _dataService.GetAccountStatusAsync(paramters);

            if (result.ResponseStatus == ResponseStatus.SuccessWithResult && result.Result != null)
            {
                AccountStatus = result.Result.Message;
                SetAccountColor(AccountStatus);
            }
            else
            {
                AccountColor = _unResolvedColor;
                string message = _messageResolver.GetMessage(result);
                if (result.ResponseStatus == ResponseStatus.InvalidToken)
                {
                    await _dialogManager.DisplayAlert(AppResources.ApplicationName, message, AppResources.Ok);
                    _navigationService.SetAppCurrentPage(typeof(LoginPage));

                }
                else
                {
                    AccountStatus = message;
                }
            }

            return result;
        }
        #endregion
    }
}
