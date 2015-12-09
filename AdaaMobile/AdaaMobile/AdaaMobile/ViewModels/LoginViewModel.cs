using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.Common;
using AdaaMobile.DataServices;
using AdaaMobile.DataServices.Requests;
using AdaaMobile.Helpers;
using AdaaMobile.Models.Request;
using AdaaMobile.Views;
using AdaaMobile.Views.MasterView;
using Xamarin.Forms;

namespace AdaaMobile.ViewModels
{
    public class LoginViewModel : BindableBase
    {

        #region Fields
        private readonly IDataService _dataService;
        private readonly IDialogManager _dialogManager;
        private readonly INavigation _navigation;
        private readonly INavigationService _navigationService;
        private readonly IAppSettings _appSettings;

        private const string LOGIN_RESPONSE_VALID_TOKEN = "Successfully Validated";
        private const string LOGIN_RESPONSE_USER_NOT_FOUND = "User Not Found";
        private const string LOGIN_RESPONSE_WRONG_PASSWORD = "Wrong Password";
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


        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                if (SetProperty(ref _userName, value))
                {
                    SetLoginCommandState();
                }
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                if (SetProperty(ref _password, value))
                {
                    SetLoginCommandState();
                }
            }
        }


        #endregion

        #region Initialization
        public LoginViewModel(IDataService dataService, IDialogManager dialogManager, INavigation navigation, INavigationService navigationService, IAppSettings appSettings)
        {
            _dataService = dataService;
            _dialogManager = dialogManager;
            _navigation = navigation;
            _navigationService = navigationService;
            _appSettings = appSettings;

            //Initialize commands
            LoginCommand = new AsyncExtendedCommand(LoginAsync, true);
            ForgetPasswordCommand = new ExtendedCommand(ForgetPassword);
            SignUpCommand = new ExtendedCommand(SignUp);

#if DEBUG
			UserName = "Mobileuser3";
			Password = "pass-123";
#endif
        }

        #endregion

        #region Commands

        public AsyncExtendedCommand LoginCommand { get; set; }
        public ExtendedCommand SignUpCommand { get; set; }
        public ExtendedCommand ForgetPasswordCommand { get; set; }
        #endregion

        #region Methods
        private async Task LoginAsync()
        {
            try
            {
                LoginCommand.CanExecute = false;
                IsBusy = true;
                BusyMessage = "Loading";
                var response = await _dataService.LoginAsync(UserName, Password);

                if (response.ResponseStatus == ResponseStatus.SuccessWithResult)
                {
                    if (response.Result.Message == LOGIN_RESPONSE_VALID_TOKEN && !string.IsNullOrEmpty(response.Result.UserToken))
                    {
                        _appSettings.UserToken = response.Result.UserToken;
                        //bool success = await GetCurrentUserProfileAsync(response.Result.UserToken);
                        //await _dialogManager.DisplayAlert("OK", response.Result.Message + "\n" + response.Result.UserToken, "Ok");

                        _navigationService.SetAppCurrentPage(typeof(AddaMasterPage));
                      //  _navigation.PushModalAsync(new AddaMasterPage());
                        return;
                    }
                    else if (response.Result.Message == LOGIN_RESPONSE_WRONG_PASSWORD ||
                        response.Result.Message == "Wrong username or password")
                    {
                        //TODO Get this from Resources file

                        await _dialogManager.DisplayAlert("Alert", "Please enter valid password", "Ok");
                        return;
                    }
                }


                await _dialogManager.DisplayAlert("Alert", "login failed", "ok");
            }
            catch (Exception)
            {
                //Show error
                _dialogManager.DisplayAlert("Error", "Something error happened", "Ok");
            }
            finally
            {
                LoginCommand.CanExecute = true;
                IsBusy = false;
            }

        }

        private async Task<bool> GetCurrentUserProfileAsync(string token)
        {
            var paramters = new CurrentProfileQParameters()
            {
                Langid = _appSettings.Language,
                UserToken = token
            };
            var result = await _dataService.GetCurrentUserProfile(paramters);

            if (result.ResponseStatus == ResponseStatus.SuccessWithResult)
            {
                LoggedUserInfo.CurrentUserProfile = result.Result;
            }
            return true;
        }

        private void SignUp()
        {
            _navigation.PushAsync(new SignUpPage());
        }

        private void ForgetPassword()
        {
            _navigation.PushAsync(new ForgetPasswordPage());

        }

        private void SetLoginCommandState()
        {
            LoginCommand.CanExecute = true;
            //!string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password);
        }

        #endregion

    }
}
