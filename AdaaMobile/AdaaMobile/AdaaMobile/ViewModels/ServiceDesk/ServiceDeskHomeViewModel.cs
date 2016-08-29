using AdaaMobile.Helpers;
using System;
using AdaaMobile.Common;
using AdaaMobile.Models;
using AdaaMobile.Strings;
using System.Collections.ObjectModel;
using AdaaMobile.Views;

namespace AdaaMobile.ViewModels
{
    public class ServiceDeskHomeViewModel
    {
        #region Fields

        private readonly INavigationService _navigationService;

        #endregion

        #region Properties

        public ObservableCollection<AdaaPageItem> PagesList { get; private set; }

        #endregion

        #region Initialization

        public ServiceDeskHomeViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            PagesList = new ObservableCollection<AdaaPageItem>() {
                new AdaaPageItem () { TargetType = typeof(MyTimesheetPage), Title = AppResources.ServiceDesk_LogAnIncident },
                new AdaaPageItem () {TargetType = typeof(MyAssigmnetsPage),Title = AppResources.ServiceDesk_RequestITService},
				new AdaaPageItem (){ TargetType = typeof(ServiceDeskRequestsPage), Title = AppResources.ServiceDesk_MyRequests },
                new AdaaPageItem (){ TargetType = typeof(ServiceDeskCasesPage), Title = AppResources.ServiceDesk_MyCases }

            };
            NavigateToPageCommand = new ExtendedCommand<Type>(NavigateToPage);
        }

        #endregion

        #region Commands

        public ExtendedCommand<Type> NavigateToPageCommand { get; set; }

        #endregion

        #region Methods

        private void NavigateToPage(Type pageType)
        {
            _navigationService.NavigateToPage(pageType);
        }

        #endregion

    }
}
