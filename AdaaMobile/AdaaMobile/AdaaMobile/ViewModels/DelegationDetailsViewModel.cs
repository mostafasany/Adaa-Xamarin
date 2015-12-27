using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AdaaMobile.Common;
using AdaaMobile.DataServices;
using AdaaMobile.DataServices.Requests;
using AdaaMobile.Helpers;
using AdaaMobile.Models.Request;
using AdaaMobile.Models.Response;
using AdaaMobile.Strings;

namespace AdaaMobile.ViewModels
{
    public class DelegationDetailsViewModel : BindableBase
    {
        #region Fields
        private readonly IDataService _dataService;
        private readonly IDialogManager _dialogManager;
        private readonly IRequestMessageResolver _messageResolver;
        private readonly INavigationService _navigationService;
        private readonly IAppSettings _appSettings;
        #endregion

        #region Properties

        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }

        private Delegation _delegation;
        public Delegation Delegation
        {
            get { return _delegation; }
            set { SetProperty(ref _delegation, value); }
        }

        #endregion

        #region Initialization
        public DelegationDetailsViewModel(IDataService dataService, IDialogManager dialogManager, IRequestMessageResolver messageResolver, INavigationService navigationService, IAppSettings appSettings)
        {
            _dataService = dataService;
            _dialogManager = dialogManager;
            _messageResolver = messageResolver;
            _navigationService = navigationService;
            _appSettings = appSettings;

            RemoveCommand=new AsyncExtendedCommand(RemoveAsync);
        }

        #endregion

        #region Commands

        public AsyncExtendedCommand RemoveCommand { get; set; }
        #endregion

        #region Methods

        private async Task RemoveAsync(CancellationToken token)
        {
            try
            {
                RemoveCommand.CanExecute = false;
                IsBusy = true;
                var parameters = new RemoveDelegationQParameter()
                {
                    Langid = _appSettings.Language,
                    UserToken = _appSettings.UserToken,
                    DelegateID = Delegation.DelegateId,
                    SubordinateID = Delegation.SubordinateId,

                };
                if (token.IsCancellationRequested)
                {
                    RemoveCommand.CanExecute = true;
                    IsBusy = false;
                    return;
                }
                var response = await _dataService.RemoveDelegationAsync(parameters);
                if (response.ResponseStatus == ResponseStatus.SuccessWithResult)
                {
                    await _dialogManager.DisplayAlert(AppResources.Error, response.Result.Message, AppResources.Ok);
                    if (token.IsCancellationRequested)
                    {
                        //Don't go Back if it's cancelled as user already pressed back button
                    }
                    else
                    {
                        _navigationService.GoBack();
                    }
                }
                else
                {
                    string message = null;
                    if (response.Result != null && !String.IsNullOrWhiteSpace(response.Result.Message))
                    {
                        message = response.Result.Message;
                    }
                    else
                    {
                        message = _messageResolver.GetMessage(response);
                    }
                    await _dialogManager.DisplayAlert(AppResources.Error, message, AppResources.Ok);
                }

            }
            catch (Exception)
            {
                RemoveCommand.CanExecute = true;
                //Show error
#pragma warning disable 4014
                _dialogManager.DisplayAlert(AppResources.ApplicationName, AppResources.SomethingErrorHappened, AppResources.Ok);
#pragma warning restore 4014
            }
            finally
            {
                IsBusy = false;
            }

        }

        #endregion

    }
}
