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

namespace AdaaMobile.Views
{
    public partial class DirectoryPage : ContentPage
    {
        private readonly DirectoryViewModel _directoryViewModel;
        private DirectoryType directoryType;
        private DirectoryPageType pageType;


        public DirectoryPage(DirectoryType directoryType, DirectoryPageType pageType)
        {
            InitializeComponent();
            this.directoryType = directoryType;
            this.pageType = pageType;

            _directoryViewModel = Locator.Default.DirectoryViewModel;
            _directoryViewModel.DirectoryType = directoryType;
            BindingContext = _directoryViewModel;
            Title = AppResources.Directory;
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
                if (pageType == DirectoryPageType.Normal)
                {
                    this.Navigation.PushAsync(new ProfilePage(emp.UserID));
                }
                else
                {
                    //TODO get back to page with selected item

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

                bool isValidSearch = await _directoryViewModel.Search(EmployeeSearchBar.Text);
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
    }
}
