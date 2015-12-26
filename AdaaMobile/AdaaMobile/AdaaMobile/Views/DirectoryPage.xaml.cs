using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.ViewModels;
using Xamarin.Forms;
using AdaaMobile.Strings;
using AdaaMobile.Models;

namespace AdaaMobile.Views
{
    public partial class DirectoryPage : ContentPage
    {
        private readonly DirectoryViewModel _directoryViewModel;
        public DirectoryPage()
        {
            InitializeComponent();
            _directoryViewModel = Locator.Default.DirectoryViewModel;
            BindingContext = _directoryViewModel;
            Title = AppResources.Directory;
            EmployeeSearchBar.TextChanged += EmployeeSearchBar_TextChanged;
			EmployeesListView.ItemTapped+= EmployeesListView_ItemTapped;

        }

        void EmployeesListView_ItemTapped (object sender, ItemTappedEventArgs e)
        {
			Employee emp = (e.Item as Employee);
			if(emp !=null)
				this.Navigation.PushAsync (new ProfilePage (emp.UserID));
        }

        private async void EmployeeSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
           if (string.IsNullOrEmpty(EmployeeSearchBar.Text))
            {
                await _directoryViewModel.GetGroupedEmployees();
				EmployeesListView.ItemsSource = _directoryViewModel.GroupedEmployees;
				EmployeesListView.IsGroupingEnabled = true;

            }
            else
            {
				bool isValidSearch = await  _directoryViewModel.Search(EmployeeSearchBar.Text);
				if (isValidSearch) {
					EmployeesListView.IsGroupingEnabled = false;
					EmployeesListView.ItemsSource = _directoryViewModel.NotGroupedEmployees;
				} else {
					
				}
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await Task.Delay(300);
            _directoryViewModel.LoadEmployeesCommand.Execute(null);
        }
    }
}
