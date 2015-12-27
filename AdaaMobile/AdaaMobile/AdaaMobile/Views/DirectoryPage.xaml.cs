using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.ViewModels;
using Xamarin.Forms;
using AdaaMobile.Strings;
using AdaaMobile.Models;
using AdaaMobile.Enums;
using AdaaMobile.Helpers;
using AdaaMobile.Models.Response;

namespace AdaaMobile.Views
{
    public partial class DirectoryPage : ContentPage, IUserSelection
    {
        private readonly DirectoryViewModel _directoryViewModel;
        private DirectorySourceType _directorySourceType;
        private DirectoryAccessType _accessType;


        public DirectoryPage(DirectorySourceType directorySourceType, DirectoryAccessType accessType)
        {
            InitializeComponent();
            Title = AppResources.Directory;
            this._directorySourceType = directorySourceType;
            this._accessType = accessType;

            _directoryViewModel = Locator.Default.DirectoryViewModel;
            _directoryViewModel.DirectorySourceType = directorySourceType;
            BindingContext = _directoryViewModel;
            EmployeeSearchBar.TextChanged += EmployeeSearchBar_TextChanged;
            EmployeesListView.ItemTapped += EmployeesListView_ItemTapped;

        }

        void EmployeesListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            //Clear Selection color
            EmployeesListView.SelectedItem = null;

            Employee emp = (e.Item as Employee);
            if (emp != null)
            {
                if (_accessType == DirectoryAccessType.Normal)
                {
                    this.Navigation.PushAsync(new ProfilePage(emp.UserId));
                }
                else
                {
                    OnUserSelected(emp);

                    this.Navigation.PopAsync();
                }
            }
        }

        private async void EmployeeSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(EmployeeSearchBar.Text))
            {
                //if (directoryType == DirectoryType.Directory)
                {
                    _directoryViewModel.GroupEmployees();
                    EmployeesListView.ItemsSource = _directoryViewModel.GroupedEmployees;
                    EmployeesListView.IsGroupingEnabled = true;
                }
                //else
                //{
                //    await _directoryViewModel.GetGroupedSubordinates();
                //    EmployeesListView.ItemsSource = _directoryViewModel.GroupedSubordinates;
                //    EmployeesListView.IsGroupingEnabled = true;
                //}
            }
            else
            {

                bool isValidSearch = _directoryViewModel.Search(EmployeeSearchBar.Text);
                if (isValidSearch)
                {
                    EmployeesListView.IsGroupingEnabled = false;
                    //if (directoryType == DirectoryType.Directory)
                    {
                        EmployeesListView.ItemsSource = _directoryViewModel.NotGroupedEmployees;
                    }
                    //else
                    //{
                    //    EmployeesListView.ItemsSource = _directoryViewModel.NotGroupedSubordinates;
                    //}
                }
                else
                {

                }

            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await Task.Delay(300);

            //Check if there data are already loaded to prevent multiple loading.
            if (_directoryViewModel.GroupedEmployees == null || _directoryViewModel.GroupedEmployees.Count == 0)
                _directoryViewModel.LoadEmployeesCommand.Execute(null);
        }

        #region User Selection Event
        public event EventHandler<Employee> UserSelected;

        protected virtual void OnUserSelected(Employee e)
        {
            var handler = UserSelected;
            if (handler != null) handler(this, e);
        }
        #endregion

        protected override bool OnBackButtonPressed()
        {
            OnUserSelected(null);
            return base.OnBackButtonPressed();          
        }
    }
}
