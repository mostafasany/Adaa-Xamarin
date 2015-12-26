using AdaaMobile.Common;
using AdaaMobile.DataServices;
using AdaaMobile.DataServices.Requests;
using AdaaMobile.Helpers;
using AdaaMobile.Models;
using AdaaMobile.Models.Request;
using AdaaMobile.Strings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaaMobile.ViewModels
{
    public class DelegationViewModel : BindableBase
    {

        #region Fields
        private readonly INavigationService _navigationService;
        private readonly IDataService _dataService;
        private readonly IAppSettings _appSettings;
        private readonly IDialogManager _dialogManager;
        private readonly IRequestMessageResolver _messageResolver;
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



        private bool _ShowNoDelegations;
        public bool ShowNoDelegations
        {
            get { return _ShowNoDelegations; }
            set { SetProperty(ref _ShowNoDelegations, value); }
        }




        private ObservableCollection<Delegation> _Delegations;
        public ObservableCollection<Delegation> Delegations
        {
            get { return _Delegations; }
            set { SetProperty(ref _Delegations, value); }
        }

        #endregion

        #region Initialization
        public DelegationViewModel(IDataService dataService, IDialogManager dialogManager, IAppSettings appSettings, INavigationService navigationService, IRequestMessageResolver messageResolver)
        {
            _dataService = dataService;
            _appSettings = appSettings;
            _navigationService = navigationService;
            _dialogManager = dialogManager;
            _messageResolver = messageResolver;
            LoadDayPassDataCommand = new AsyncExtendedCommand(LoadDelagationDataAsync);
            NewDelegationCommand = new AsyncExtendedCommand(DoNewDelegationCommand);

        }

        #endregion

        #region Commands
        public AsyncExtendedCommand LoadDayPassDataCommand { get; set; }
        public AsyncExtendedCommand NewDelegationCommand { get; set; }
        #endregion

        #region Methods
        private async Task LoadDelagationDataAsync()
        {
            try
            {
                IsBusy = true;
                LoadDayPassDataCommand.CanExecute = false;
                ShowNoDelegations = false;
                var response = await _dataService.GetAllDelegationsResponseAsync(new DelegationsQParamters()
                {
                    Langid = _appSettings.Language,
                    UserToken = _appSettings.UserToken
                });

                if (response.ResponseStatus == ResponseStatus.SuccessWithResult && response.Result != null)
                {
                    if (response.Result.Delegations != null && response.Result.Delegations.Length > 0)
                    {
                        Delegations = new ObservableCollection<Delegation>(response.Result.Delegations);
                        ShowNoDelegations = false;
                    }
                    else
                    {
                        Delegations = null;
                        ShowNoDelegations = true;
                    }

                }
            }
            catch (Exception)
            {
                // ignored
            }
            finally
            {
                LoadDayPassDataCommand.CanExecute = true;
                IsBusy = false;
            }
        }


        private async Task DoNewDelegationCommand()
        {
            try
            {
                NewDelegationCommand.CanExecute = false;
                IsBusy = true;
                BusyMessage = AppResources.Loading;
               // if (string.IsNullOrEmpty(Reason))
                //{
                //    //await _dialogManager.DisplayAlert(AppResources.ApplicationName, AppResources.PleaseenterUserName, AppResources.Ok);
                //}
               // else
                {
                    var qParamters = new NewDelegationQParameter ()
                    {
                        Langid = _appSettings.Language,
                        UserToken = _appSettings.UserToken,
                        DelegateID = "",
                        SubordinateID = ""
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
        #endregion
    }
}
