using AdaaMobile.Common;
using AdaaMobile.DataServices;
using AdaaMobile.DataServices.Requests;
using AdaaMobile.Helpers;
using AdaaMobile.Models.Request;
using AdaaMobile.Strings;
using AdaaMobile.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        #endregion

        #region Properties
        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }


        private string _AccountStatus;
        public string AccountStatus
        {
            get { return _AccountStatus; }
            set { SetProperty(ref _AccountStatus, value); }
        }


        private string _PasswordStatus;
        public string PasswordStatus
        {
            get { return _PasswordStatus; }
            set { SetProperty(ref _PasswordStatus, value); }
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
            NavigateToChangePasswordCommand = new AsyncExtendedCommand(DoNavigateToChangePassword);
        }


        private async Task LoadAsync()
        {
            try
            {
                LoadCommand.CanExecute = false;
                IsBusy = true;
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
                    }
                    else
                    {
                        string message = _messageResolver.GetMessage(result);
                        await _dialogManager.DisplayAlert(AppResources.Alert, message, AppResources.Ok);
                        if (result.ResponseStatus == ResponseStatus.InvalidToken)
                        {
                            _navigationService.SetAppCurrentPage(typeof(LoginPage));

                        }
                    }

                    var passwordparamters = new PasswordStatusQParameters()
                    {
                        Langid = _appSettings.Language,
                        UserToken = _appSettings.UserToken
                    };
                    var passwordresult = await _dataService.GetPasswordStatusAsync(passwordparamters);

                    if (passwordresult.ResponseStatus == ResponseStatus.SuccessWithResult && passwordresult.Result != null)
                    {
                        PasswordStatus = passwordresult.Result.Message;
                    }
                    else
                    {
                        string message = _messageResolver.GetMessage(passwordresult);
                        await _dialogManager.DisplayAlert(AppResources.Alert, message, AppResources.Ok);
                        if (result.ResponseStatus == ResponseStatus.InvalidToken)
                        {
                            _navigationService.SetAppCurrentPage(typeof(LoginPage));
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                LoadCommand.CanExecute = true;
                IsBusy = false;
            }

        }

        #endregion

        #region Commands
        public AsyncExtendedCommand LoadCommand { get; set; }
        public AsyncExtendedCommand NavigateToChangePasswordCommand { get; set; }
        public AsyncExtendedCommand UnlockMyAccountCommand { get; set; }
        #endregion

        #region Methods
        private async Task DoNavigateToChangePassword()
        {
            _navigationService.SetAppCurrentPage(typeof(ChangePasswordPage));
        }

        private async Task UnlockMyAccountAsync()
        {
            try
            {
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
        #endregion
    }
}
