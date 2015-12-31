using System;
using AdaaMobile.DataServices.Requests;
using AdaaMobile.Models;
using AdaaMobile.Models.Response;
using AdaaMobile.ViewModels;
using Xamarin.Forms;

namespace AdaaMobile.Views.DayPass
{
    public partial class DayPassTaskDetailsPage : ContentPage
    {
        private DayPassTask task;
        private TaskDetailsViewmodel _taskDetailsViewmodel;

        public DayPassTaskDetailsPage(DayPassTask task)
        {
            InitializeComponent();
            this.task = task;
            _taskDetailsViewmodel = ViewModels.Locator.Default.TaskDetailsViewmodel;
            _taskDetailsViewmodel.CurrentTask = task;
            BindingContext = _taskDetailsViewmodel;

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                ResponseWrapper<UserProfile> resposne = await _taskDetailsViewmodel.LoadProfileAsync(task.UserId);
                if (resposne.ResponseStatus == ResponseStatus.SuccessWithResult)
                {
                    if (resposne.Result != null)
                    {
                        _taskDetailsViewmodel.CurrentTask.Departement = resposne.Result.DeptName;
                    }
                }
            }
            catch (Exception)
            {
                //ignored
            }
        }



    }
}
