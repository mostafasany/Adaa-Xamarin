using System;
using AdaaMobile.DataServices.Requests;
using AdaaMobile.Models;
using AdaaMobile.Models.Response;
using AdaaMobile.ViewModels;
using Xamarin.Forms;
using AdaaMobile.Strings;

namespace AdaaMobile.Views.DayPass
{
    public partial class DayPassTaskDetailsPage : ContentPage
    {
        private DayPassTask task;
        private TaskDetailsViewmodel _taskDetailsViewmodel;

        public DayPassTaskDetailsPage(DayPassTask task)
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");
            this.task = task;
            _taskDetailsViewmodel = ViewModels.Locator.Default.TaskDetailsViewmodel;
            _taskDetailsViewmodel.CurrentTask = task;
            BindingContext = _taskDetailsViewmodel;
            Title = AppResources.TaskDetails;
			ApproveBtn.Clicked += ApproveBtn_Clicked;
			RejectBtn.Clicked+= RejectBtn_Clicked;
        }

        void RejectBtn_Clicked (object sender, EventArgs e)
        {
			_taskDetailsViewmodel.RejectCommand.Execute (null);
        }

        void ApproveBtn_Clicked (object sender, EventArgs e)
        {
			_taskDetailsViewmodel.ApproveCommand.Execute (null);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            NavigationPage.SetBackButtonTitle(this, string.Empty);
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
