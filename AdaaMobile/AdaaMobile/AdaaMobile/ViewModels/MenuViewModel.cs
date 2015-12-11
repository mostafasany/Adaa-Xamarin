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
    public class MenuViewModel : BindableBase
    {
        private readonly INavigationService _navigationService;
        #region Fields
        #endregion

        #region Properties
        private List<AdaaPageItem> _pages;

        public List<AdaaPageItem> Pages
        {
            get { return _pages; }
            set { SetProperty(ref _pages, value); }
        }

        #endregion

        #region Initialization
        public MenuViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            PageClickedCommand = new ExtendedCommand<AdaaPageItem>(PageItemClicked);

        }
        #endregion

        #region Commands
        public ExtendedCommand<AdaaPageItem> PageClickedCommand { get; set; }
        #endregion

        #region Methods
        private void PageItemClicked(AdaaPageItem item)
        {
            if (item == null)
                return;
            _navigationService.SetMasterDetailsPage(item.TargetType);
        }
        #endregion

    }
}
