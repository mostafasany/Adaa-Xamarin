using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.Common;
using AdaaMobile.DataServices;

namespace AdaaMobile.ViewModels
{
    public class DirectoryViewModel : BindableBase
    {

        #region Fields
        private readonly IDataService _dataService;
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

        #endregion

        #region Initialization
        public DirectoryViewModel(IDataService dataService)
        {
            _dataService = dataService;
        }

        #endregion

        #region Commands
        #endregion

        #region Methods
        #endregion

    }
}
