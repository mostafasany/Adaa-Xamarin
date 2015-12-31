using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
    public class TaskDetailsViewmodel : BindableBase
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

        private DayPassTask _currentTask;
        public DayPassTask CurrentTask
        {
            get { return _currentTask; }
            set { SetProperty(ref _currentTask, value); }
        }

        #endregion

        #region Initialization
        public TaskDetailsViewmodel(IDataService dataService, IDialogManager dialogManager, IAppSettings appSettings, INavigationService navigationService, IRequestMessageResolver messageResolver)
        {
            _dataService = dataService;
            _appSettings = appSettings;
            _navigationService = navigationService;
            _dialogManager = dialogManager;
            _messageResolver = messageResolver;

        }

        #endregion

        #region Commands

        #endregion

        #region Methods
        public async Task<ResponseWrapper<UserProfile>> LoadProfileAsync(string empId)
        {
            try
            {

                IsBusy = true;
                //Load other profile
                var parameters = new OtherProfileQParameters()
                {
                    Langid = _appSettings.Language,
                    UserToken = _appSettings.UserToken,
                    EmpId = empId
                };

                return await _dataService.GetOtherUserProfile(parameters);
            }
            catch (Exception)
            {
                return new ResponseWrapper<UserProfile>();
            }
            finally
            {
                IsBusy = false;
            }
        }
        #endregion

    }
}
