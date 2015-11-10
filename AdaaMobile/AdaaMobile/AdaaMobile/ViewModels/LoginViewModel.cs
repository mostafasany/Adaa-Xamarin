using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.Common;
using AdaaMobile.DataServices;
using AdaaMobile.DataServices.Requests;
using AdaaMobile.Helpers;
using AdaaMobile.Views;
using Xamarin.Forms;

namespace AdaaMobile.ViewModels
{
    public class LoginViewModel : BindableBase
    {

        #region Fields
        private readonly IDataService _dataService;
        private readonly IDialogManager _dialogManager;
        private readonly INavigation _navigation;
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
        public LoginViewModel(IDataService dataService, IDialogManager dialogManager, INavigation navigation)
        {
            _dataService = dataService;
            _dialogManager = dialogManager;
            _navigation = navigation;

            //Initialize commands
            LoginCommand = new AsyncExtendedCommand(LoginAsync, false);

#if DEBUG
            UserName = "Test";
            Password = "123456";
#endif
        }

        #endregion

        #region Commands

        public AsyncExtendedCommand LoginCommand { get; set; }
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
                    if (response.Result.Status == "ok")
                    {
                        //sucess
                        //await _navigation.PopModalAsync(false);
                        //await _navigation.PushAsync(new MasterPage(), true);
                        App.Current.MainPage=new MasterPage();
                        return;
                    }
                }

                await _dialogManager.DisplayAlert("Alert", "login failed", "ok");
            }
            catch (Exception ex)
            {
                //Show error
            }
            finally
            {
                LoginCommand.CanExecute = true;
                IsBusy = false;
            }

        }

        private void SetLoginCommandState()
        {
            LoginCommand.CanExecute = !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password);
        }

#endregion

    }
}
