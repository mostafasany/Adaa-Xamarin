using AdaaMobile.Common;
using AdaaMobile.DataServices;
using AdaaMobile.DataServices.Requests;
using AdaaMobile.Helpers;
using AdaaMobile.Models.Response;
using AdaaMobile.Strings;
using AdaaMobile.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdaaMobile.ViewModels
{
    public class ChangePasswordViewModel : BindableBase
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

        private string _NewPassword;
        public string NewPassword
        {
            get { return _NewPassword; }
            set { SetProperty(ref _NewPassword, value); }
        }

        private string _ConfirmPassword;
        public string ConfirmPassword
        {
            get { return _ConfirmPassword; }
            set { SetProperty(ref _ConfirmPassword, value); }
        }
        #endregion

        #region Initialization

        public ChangePasswordViewModel(IDataService dataService, IAppSettings appSettings, INavigationService navigationService, IRequestMessageResolver messageResolver, IDialogManager dialogManager)
        {
            _dataService = dataService;
            _appSettings = appSettings;
            _navigationService = navigationService;
            _messageResolver = messageResolver;
            _dialogManager = dialogManager;
            LoadCommand = new AsyncExtendedCommand(LoadAsync);
            ChangePasswordCommand = new AsyncExtendedCommand(DoChangePassword);
        }


        private async Task LoadAsync()
        {
            try
            {
                LoadCommand.CanExecute = false;
                IsBusy = true;
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
        public AsyncExtendedCommand ChangePasswordCommand { get; set; }

        #endregion

        #region Methods
        private async Task DoChangePassword()
        {
            try
            {
                ChangePasswordCommand.CanExecute = false;
                IsBusy = true;
                if (string.IsNullOrEmpty(NewPassword) || string.IsNullOrEmpty(ConfirmPassword))
                {
                    await _dialogManager.DisplayAlert(AppResources.ApplicationName, AppResources.PleaseEnterPassAndConfirmPass, AppResources.Ok);
                }
                else if (NewPassword.Length < 8 || ConfirmPassword.Length < 8)
                {
                    await _dialogManager.DisplayAlert(AppResources.ApplicationName, AppResources.PassLessThan8CharsMessage, AppResources.Ok);
                }
                else if (NewPassword != ConfirmPassword)
                {
                    await _dialogManager.DisplayAlert(AppResources.ApplicationName, AppResources.PassAndConfirmPassAreNotEqual, AppResources.Ok);
                }
				else if (!IsValaidPassword(NewPassword))
                {
                    await _dialogManager.DisplayAlert(AppResources.ApplicationName, AppResources.PassShouldnotContainSpecialChars, AppResources.Ok);
                }
                else
                {
                    var result = await _dataService.ChangePasswordAsync(NewPassword, new Models.Request.ChangePasswordQParameters()
                    {
                        UserToken = _appSettings.UserToken,
                        Langid = _appSettings.Language
                    });

                    if (result.ResponseStatus == ResponseStatus.SuccessWithResult && result.Result != null)
                    {

                        await _dialogManager.DisplayAlert(AppResources.ApplicationName, result.Result.Message, AppResources.Ok);
                        if (result.Result.Message == "Password Changed Successfully")
                        {
                            _navigationService.GoBack();
                        }
                    }
                    else
                    {
                        string message = _messageResolver.GetMessage(result);
                        await _dialogManager.DisplayAlert(AppResources.ApplicationName, message, AppResources.Ok);
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
				ChangePasswordCommand.CanExecute = true;
                IsBusy = false;
            }
        }



        private bool IsValaidPassword(string value)
        {
			var regexItem = new Regex(@"^[a-z0-9.@#$\-]+$");

            return regexItem.IsMatch(value);
        }

        #endregion
    }
}
