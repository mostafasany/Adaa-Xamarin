using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.Common;
using AdaaMobile.Helpers;
using AdaaMobile.Models;

namespace AdaaMobile.ViewModels
{
    public class HomeViewModel : BindableBase
    {

        #region Fields
        private readonly INavigationService _navigationService;
        #endregion

        #region Properties
        public List<AdaaPageItem> Pages { get; set; }
        #endregion

        #region Initialization
        public HomeViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            Pages = AdaaPageItem.Pages;
            PageClickedCommand = new ExtendedCommand<AdaaPageItem>(PageItemClicked);
        }
        #endregion

        #region Commands

        public ExtendedCommand<AdaaPageItem> PageClickedCommand { get; set; }
        #endregion

        #region Methods

        private void PageItemClicked(AdaaPageItem item)
        {
            if (item == null) return;
            _navigationService.SetMasterDetailsPage(item.TargetType);
        }

        #endregion

    }
}
