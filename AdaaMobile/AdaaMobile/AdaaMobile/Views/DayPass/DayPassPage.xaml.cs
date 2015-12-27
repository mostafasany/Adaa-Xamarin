﻿using System;
using AdaaMobile.Models;
using AdaaMobile.Models.Response;
using AdaaMobile.Strings;
using AdaaMobile.ViewModels;
using Xamarin.Forms;

namespace AdaaMobile.Views.DayPass
{
    public partial class DayPassPage : ContentPage
    {

        private Button _lastTappedTab;
        private DayPassViewModel _dayPassViewModel;

        public DayPassPage()
        {
            InitializeComponent();
            Title = AppResources.DayPass;
            _dayPassViewModel = ViewModels.Locator.Default.DayPassViewModel;
            BindingContext = _dayPassViewModel;
            SelectButton(MyRequestsButton, true);
            SelectButton(MyTasksButton, false);
            _lastTappedTab = MyRequestsButton;

            MyRequestsList.ItemTapped += MyRequestsList_ItemTapped;
            MyTasksList.ItemTapped += MyTasksList_ItemTapped;

            Action action = () =>
            {
                this.Navigation.PushAsync(new NewDayPassRequestPage());
            };
            ToolbarItems.Add(
                new ToolbarItem("Add", "", action, ToolbarItemOrder.Primary));
        }

        private void MyTasksList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            DayPassRequest request = (e.Item as DayPassRequest);
            _dayPassViewModel.SelectedRequest = request;
            this.Navigation.PushAsync(new DayPassRequestDetailsPage(request));
        }

        private void MyRequestsList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            DayPassTask task = (e.Item as DayPassTask);
            _dayPassViewModel.SelectedTask = task;
            this.Navigation.PushAsync(new DayPassTaskDetailsPage(task));
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                _dayPassViewModel.LoadDayPassDataCommand.Execute(null);
            }
            catch (Exception ex)
            {
                //ignored
            }
        }


        #region Tabs Methods
        private void SelectButton(Button button, bool selected)
        {
            var darkColor = (Color)Resources["TabColor"];

            if (selected)
            {
                button.BackgroundColor = darkColor;
                button.TextColor = Color.White;
            }
            else
            {
                button.BackgroundColor = Color.White;
                button.TextColor = darkColor;
            }
        }

        /// <summary>
        /// This method changes button backgroundcolor and textcolor based on pressed state.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTabTapped(object sender, EventArgs e)
        {
            var button = (Button)sender;
            if (button == _lastTappedTab) return;
            //Change color state of tapped button to Active
            SelectButton(button, true);
            //Change color state of tapped button to be not Active
            if (_lastTappedTab != null)
            {
                SelectButton(_lastTappedTab, false);
            }
            _lastTappedTab = button;

            //Switch to different modes based on tapped button
            if (button == MyRequestsButton)
            {
                MyRequestsListGrid.IsVisible = true;
                MyTasksListGrid.IsVisible = false;
            }
            else if (button == MyTasksButton)
            {
                MyRequestsListGrid.IsVisible = false;
                MyTasksListGrid.IsVisible = true;
            }
        }

        #endregion
    }
}