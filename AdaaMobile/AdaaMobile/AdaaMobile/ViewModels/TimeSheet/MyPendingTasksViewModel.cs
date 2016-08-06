using AdaaMobile.DataServices;
using AdaaMobile.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdaaMobile.Common;
using AdaaMobile.Models.Response;
using AdaaMobile.DataServices.Requests;
using Xamarin.Forms;

namespace AdaaMobile.ViewModels
{
    public class MyPendingTasksViewModel : BindableBase
    {
        #region Fields

        private readonly INavigationService _navigationService;
        private readonly IDataService _dataService;

        #endregion

        #region Properties

        private List<PendingTask> _PendingTask;

        public List<PendingTask> PendingTask
        {
            get { return _PendingTask; }
            set { SetProperty(ref _PendingTask, value); OnPropertyChanged("NoPendingTasks"); }
        }

        public bool NoPendingTasks
        {
            get
            {
                if (PendingTask != null && PendingTask.Count > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        private PendingTask _SelectedPendingTask;

        public PendingTask SelectedPendingTask
        {
            get { return _SelectedPendingTask; }
            set { SetProperty(ref _SelectedPendingTask, value); }
        }

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

        public MyPendingTasksViewModel(INavigationService navigationService, IDataService dataservice)
        {
            _navigationService = navigationService;
            _dataService = dataservice;
            PageLoadedCommand = new AsyncExtendedCommand(Loaded);
            RequestItemSelectedCommand = new AsyncExtendedCommand<PendingTask>(OpenRequestDetailsPage);
            OpenTaskCommand = new AsyncExtendedCommand(OpenTaskWebView);
        }

        #endregion

        #region Commands

        public AsyncExtendedCommand PageLoadedCommand { get; set; }

        public AsyncExtendedCommand<PendingTask> RequestItemSelectedCommand { get; set; }

        public AsyncExtendedCommand OpenTaskCommand { get; set; }

        #endregion

        #region Methods

        private async Task Loaded()
        {
            try
            {
                IsBusy = true;
                var response = await _dataService.GetPendingTaskAsync(null);
                if (response.ResponseStatus == ResponseStatus.SuccessWithResult && response.Result != null)
                {
                    if (response.Result != null && response.Result.Count > 0)
                    {
                        PendingTask = response.Result;
                    }
                    else
                    {

                    }
                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsBusy = false;
            }

        }

        private async Task OpenRequestDetailsPage(PendingTask pendingTask)
        {
            SelectedPendingTask = pendingTask;
            _navigationService.NavigateToPage(typeof(SelectedPendingTaskPage));
        }

        private async Task OpenTaskWebView()
        {
            var selectedtask = SelectedPendingTask;
			var fullURL = "http://adaatime.linkdev.com" + selectedtask.TaskFullURL;
			Device.OpenUri(new Uri(fullURL, UriKind.Absolute));
        }

        #endregion

    }
}
