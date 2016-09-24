using AdaaMobile.DataServices;
using AdaaMobile.Helpers;
using System;
using System.Threading.Tasks;
using AdaaMobile.Common;
using AdaaMobile.Models;
using System.Collections.ObjectModel;
using AdaaMobile.Models.Response.ServiceDesk;

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


        //private ObservableCollection<OnBehalf> _OnBehalfList;
        //public ObservableCollection<OnBehalf> OnBehalfList
        //{
        //    get { return _OnBehalfList; }
        //    set { SetProperty(ref _OnBehalfList, value); }
        //}

        //private ObservableCollection<ParentCategory> _ParentCategoryList;
        //public ObservableCollection<ParentCategory> ParentCategoryList
        //{
        //    get { return _ParentCategoryList; }
        //    set { SetProperty(ref _ParentCategoryList, value); }
        //}

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
                //var onbelfResult = await _dataService.GetOnBelfUsers(null);
                //if (onbelfResult != null && onbelfResult.Result != null)
                //    OnBehalfList = new ObservableCollection<OnBehalf>(onbelfResult.Result.result);

                //var parentCategoryResult = await _dataService.GetParentCategories(null);
                //if (parentCategoryResult != null && parentCategoryResult.Result != null)
                //    ParentCategoryList = new ObservableCollection<ParentCategory>(parentCategoryResult.Result.result);

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
