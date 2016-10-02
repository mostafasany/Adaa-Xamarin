using AdaaMobile.Helpers;
using AdaaMobile.Common;
using AdaaMobile.Models;
using AdaaMobile.Strings;
using System.Collections.ObjectModel;

namespace AdaaMobile.ViewModels
{
    public class ServiceDeskHomeViewModel
    {
        #region Fields

        private readonly INavigationService _navigationService;

        #endregion

        #region Properties

        public ObservableCollection<AdaaPageItem> PagesList { get; private set; }
        public string Module { get; private set; }

        #endregion

        #region Initialization

        public ServiceDeskHomeViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            PagesList = new ObservableCollection<AdaaPageItem>() {
                new AdaaPageItem () { TargetType = typeof(ServiceDeskLogIncident), Title = AppResources.ServiceDesk_LogAnIncident },
                new AdaaPageItem () {TargetType = typeof(ServiceDeskLogIncident),Title = AppResources.ServiceDesk_RequestITService},
                new AdaaPageItem (){ TargetType = typeof(ServiceDeskRequestsPage), Title = AppResources.ServiceDesk_MyRequests },
                new AdaaPageItem (){ TargetType = typeof(ServiceDeskCasesPage), Title = AppResources.ServiceDesk_MyCases }
            };
            NavigateToPageCommand = new ExtendedCommand<AdaaPageItem>(NavigateToPage);
        }

        #endregion

        #region Commands

        public ExtendedCommand<AdaaPageItem> NavigateToPageCommand { get; set; }

        #endregion

        #region Methods

        private void NavigateToPage(AdaaPageItem pageType)
        {
            if (pageType.Title == AppResources.ServiceDesk_LogAnIncident)
            {
                Module = "Incident%20Classification";
            }
            else if (pageType.Title == AppResources.ServiceDesk_RequestITService)
            {
                Module = "Service%20request%20area";
            }
            else
            {
                Module = "";
            }
            _navigationService.NavigateToPage(pageType.TargetType);
        }

        #endregion

    }
}
