using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.Common;
using AdaaMobile.DataServices;

namespace AdaaMobile.ViewModels
{
    public class LoginViewModel : BindableBase
    {

        #region Fields
        private readonly IDataService _dataService;
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
            set { SetProperty(ref _userName, value); }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }


        #endregion

        #region Initialization
        public LoginViewModel(IDataService dataService)
        {
            _dataService = dataService;

            //Initialize commands
            LoginCommand = new AsyncExtendedCommand(LoginAsync);
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

            }
            catch (Exception)
            {
                //Show error
            }
            finally
            {
                LoginCommand.CanExecute = true;
                IsBusy = false;
            }

        }
        #endregion

    }
}
