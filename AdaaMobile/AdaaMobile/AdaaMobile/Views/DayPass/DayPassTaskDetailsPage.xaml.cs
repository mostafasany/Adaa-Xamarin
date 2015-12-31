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
        private DayPassViewModel _dayPassViewModel;

        public DayPassTaskDetailsPage(DayPassTask task)
        {
            InitializeComponent();
            this.task = task;
            _dayPassViewModel = ViewModels.Locator.Default.DayPassViewModel;
            BindingContext = _dayPassViewModel;
            DepartureTimeLabel.Text = task.StartTime;
           
            ExpectedReturnTimeLabel.Text = task.EndTime;
            ReasonTypeLabel.Text = task.ReasonType;
            

            EmployeeIdLabel.Text = task.UserId;
            FullNameLabel.Text = task.UserName;



        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
			NavigationPage.SetBackButtonTitle (this, string.Empty);
            try
            {
                ResponseWrapper<UserProfile> resposne = await _dayPassViewModel.LoadProfileAsync(task.UserId);
                if (resposne.ResponseStatus == ResponseStatus.SuccessWithResult)
                {
                    if (resposne.Result != null)
                    {
                        DepartmentLabel.Text = resposne.Result.DeptName;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }



    }
}
