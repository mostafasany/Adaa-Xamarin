using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.Common;
using AdaaMobile.DataServices;
using AdaaMobile.DataServices.Requests;
using AdaaMobile.Enums;
using AdaaMobile.Helpers;
using AdaaMobile.Models.Request;
using AdaaMobile.Models.Response;
using AdaaMobile.Strings;

namespace AdaaMobile.ViewModels
{
    public class NewDelegationViewModel : BindableBase
    {
        #region Fields

        private readonly IDialogManager _dialogManager;
        private readonly IDataService _dataService;
        private readonly IAppSettings _appSettings;
        private readonly IRequestMessageResolver _messageResolver;
        private readonly UserSelectionService _userSelectionService;
        #endregion

        #region Properties

        private Employee _userDelegate;

        public Employee UserDelegate
        {
            get { return _userDelegate; }
            set
            {
                if (SetProperty(ref _userDelegate, value))
                {
                    OnPropertyChanged(@"DelegateName");
                }
            }
        }

        private Employee _userSubOrdinate;

        public Employee UserSubOrdinate
        {
            get { return _userSubOrdinate; }
            set
            {
                if (SetProperty(ref _userSubOrdinate, value))
                {
                    OnPropertyChanged(@"SubOrdinateName");
                }
            }
        }

        private bool _ruleStatus;
        public bool RuleStatus
        {
            get { return _ruleStatus; }
            set { SetProperty(ref _ruleStatus, value); }
        }


        public string DelegateName
        {
            get { return UserDelegate == null ? AppResources.EmptyPlaceHolder : UserDelegate.UserName; }
        }

        public string SubOrdinateName
        {
            get { return UserSubOrdinate == null ? AppResources.EmptyPlaceHolder : UserSubOrdinate.UserName; }
        }



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
        #endregion

        #region Initialization
        public NewDelegationViewModel(IDataService dataService, IDialogManager dialogManager, IAppSettings appSettings, IRequestMessageResolver messageResolver, UserSelectionService userSelectionService)
        {
            _dataService = dataService;
            _dialogManager = dialogManager;
            _appSettings = appSettings;
            _messageResolver = messageResolver;
            _userSelectionService = userSelectionService;

            NewDelegationCommand = new AsyncExtendedCommand(NewDelegateAsync);
            SelectProfileCommand = new AsyncExtendedCommand<string>(SelectProfileAsync);
        }

        #endregion

        #region Commands
        public AsyncExtendedCommand NewDelegationCommand { get; set; }
        public AsyncExtendedCommand<string> SelectProfileCommand { get; set; }

        #endregion

        #region Methods

        private async Task NewDelegateAsync()
        {
            try
            {
                if (UserDelegate == null || UserSubOrdinate == null) return;
                NewDelegationCommand.CanExecute = false;
                IsBusy = true;
                BusyMessage = AppResources.Loading;

                var qParamters = new NewDelegationQParameter()
                {
                    Langid = _appSettings.Language,
                    UserToken = _appSettings.UserToken,
                    DelegateID = UserDelegate.UserId + "",
                    SubordinateID = UserSubOrdinate.UserId + "",

                };
                var response = await _dataService.NewDelegationAsync(qParamters);

                if (response.ResponseStatus == ResponseStatus.SuccessWithResult)
                {
                    if (!string.IsNullOrEmpty(response.Result.Message))
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
            catch (Exception ex)
            {
                //Show error
#pragma warning disable 4014
                _dialogManager.DisplayAlert(AppResources.ApplicationName, AppResources.SomethingErrorHappened, AppResources.Ok);
#pragma warning restore 4014
            }
            finally
            {
                NewDelegationCommand.CanExecute = true;
                IsBusy = false;
            }
        }

        private async Task SelectProfileAsync(string delegateLiteral)
        {
            var delegateType = (DirectorySourceType)Enum.Parse(typeof(DirectorySourceType), delegateLiteral);

            //Select User.
            var user = await _userSelectionService.SelectUserAsync(delegateType);
            if (user == null) return;
            //Assign to delegate or subordinate
            switch (delegateType)
            {
                case DirectorySourceType.Directory:
                    UserDelegate = user;
                    break;
                case DirectorySourceType.Subordinats:
                    UserSubOrdinate = user;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion
    }

}
