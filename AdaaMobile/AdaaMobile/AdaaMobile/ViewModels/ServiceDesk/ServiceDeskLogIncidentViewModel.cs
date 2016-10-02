using AdaaMobile.DataServices;
using AdaaMobile.Helpers;
using System;
using System.Threading.Tasks;
using AdaaMobile.Common;

namespace AdaaMobile.ViewModels
{
    public class ServiceDeskLogIncidentViewModel : BindableBase
    {
        #region Fields

        private readonly INavigationService _navigationService;
        private readonly IDataService _dataService;
        #endregion

        #region Properties

        private bool _IsBusy;
        public bool IsBusy
        {
            get { return _IsBusy; }
            set
            {
                SetProperty(ref _IsBusy, value);
            }
        }

        #endregion

        #region Initialization

        public ServiceDeskLogIncidentViewModel(INavigationService navigationService, IDataService dataservice)
        {
            _navigationService = navigationService;
            _dataService = dataservice;
            PageLoadedCommand = new AsyncExtendedCommand(Loaded);
        }

        #endregion

        #region Commands
        public AsyncExtendedCommand PageLoadedCommand { get; set; }
        #endregion

        #region Methods

        private async Task Loaded()
        {
            try
            {
                IsBusy = true;
            }
            catch (Exception ex)
            {
            }
            finally
            {
                IsBusy = false;
            }

        }

        #endregion

    }
}
