﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.Models;
using Xamarin.Forms;
using AdaaMobile.ViewModels;
using AdaaMobile.DataServices.Requests;
using AdaaMobile.Models.Response;

namespace AdaaMobile.Views
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
            

            EmployeeIdLabel.Text = task.UserID;
            FullNameLabel.Text = task.UserName;



        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                ResponseWrapper<UserProfile> resposne = await _dayPassViewModel.LoadProfileAsync(task.UserID);
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
