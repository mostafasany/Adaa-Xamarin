using System;
using System.Threading.Tasks;
using AdaaMobile.Common;
using AdaaMobile.DataServices;
using AdaaMobile.DataServices.Requests;
using AdaaMobile.Helpers;
using AdaaMobile.Models.Request;
using AdaaMobile.Strings;
using AdaaMobile.Views;
using AdaaMobile.Views.Authentication;
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
        private readonly IRequestMessageResolver _messageResolver;
        private const string LoginResponseValidToken = "Successfully Validated";
        private const string LoginResponseUserNotFound = "User Not Found";
        private const string LoginResponseWrongPassword = "Wrong Password";
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
        public LoginViewModel(IDataService dataService, IDialogManager dialogManager, INavigation navigation, INavigationService navigationService, IAppSettings appSettings, IRequestMessageResolver messageResolver)
        {
            _dataService = dataService;
            _dialogManager = dialogManager;
            _navigation = navigation;
            _navigationService = navigationService;
            _appSettings = appSettings;
            _messageResolver = messageResolver;

            //Initialize commands
            LoginCommand = new AsyncExtendedCommand(LoginAsync, true);
            ForgetPasswordCommand = new ExtendedCommand(ForgetPassword);
            SignUpCommand = new ExtendedCommand(SignUp);

#if DEBUG
            UserName = "Mobileuser1";
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
                BusyMessage = AppResources.Loading;
                if (string.IsNullOrEmpty(UserName))
                {
                    await _dialogManager.DisplayAlert(AppResources.ApplicationName, AppResources.PleaseenterUserName, AppResources.Ok);
                }
                else if (string.IsNullOrEmpty(UserName) && string.IsNullOrEmpty(Password))
                {
                    await _dialogManager.DisplayAlert(AppResources.ApplicationName, AppResources.PleaseenterUserNameandPassword, AppResources.Ok);
                }
                else
                {
					//_navigationService.SetAppCurrentPage(typeof(AddaMasterPage));
                    var response = await _dataService.LoginAsync(UserName, Password);

                    if (response.ResponseStatus == ResponseStatus.SuccessWithResult)
                    {
                        if (response.Result.Message == LoginResponseValidToken && !string.IsNullOrEmpty(response.Result.UserToken))
                        {
                            _appSettings.UserToken = response.Result.UserToken;
                            _navigationService.SetAppCurrentPage(typeof(AddaMasterPage));
                        }
                        else if (response.Result.Message == LoginResponseWrongPassword ||
                                 response.Result.Message == "Wrong username or password")
                        {
                            await _dialogManager.DisplayAlert(AppResources.ApplicationName, AppResources.EnterValidPassword, AppResources.Ok);
                        }
                    }
                    else
                    {
                        string message = _messageResolver.GetMessage(response);
                        await _dialogManager.DisplayAlert(AppResources.ApplicationName, message, AppResources.Ok);
                    }
                }

            }
            catch (Exception ex)
            {
                //Show error
#pragma warning disable 4014
                _dialogManager.DisplayAlert(AppResources.ApplicationName, AppResources.SomethingErrorHappened, AppResources.Ok);
#pragma warning restore 4014
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
