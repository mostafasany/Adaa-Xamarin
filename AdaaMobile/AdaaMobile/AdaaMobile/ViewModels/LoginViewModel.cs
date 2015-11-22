﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.Common;
using AdaaMobile.DataServices;
using AdaaMobile.DataServices.Requests;
using AdaaMobile.Helpers;
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
        public LoginViewModel(IDataService dataService, IDialogManager dialogManager, INavigation navigation, INavigationService navigationService)
        {
            _dataService = dataService;
            _dialogManager = dialogManager;
            _navigation = navigation;
            _navigationService = navigationService;

            //Initialize commands
            LoginCommand = new AsyncExtendedCommand(LoginAsync, false);
            ForgetPasswordCommand = new ExtendedCommand(ForgetPassword);
            SignUpCommand = new ExtendedCommand(SignUp);

#if DEBUG
            UserName = "Test";
            Password = "123456";
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
					if (response.Result.Status == "ok" && !string.IsNullOrEmpty( response.Result.UserToken))
                    {
						new AppSettings().UserToken = response.Result.UserToken;
                        _navigationService.SetAppCurrentPage(typeof(AddaMasterPage));
                        return;
                    }
                }

                await _dialogManager.DisplayAlert("Alert", "login failed", "ok");
            }
            catch (Exception )
            {
                //Show error
            }
            finally
            {
                LoginCommand.CanExecute = true;
                IsBusy = false;
            }

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
            LoginCommand.CanExecute = !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password);
        }

        #endregion

    }
}
