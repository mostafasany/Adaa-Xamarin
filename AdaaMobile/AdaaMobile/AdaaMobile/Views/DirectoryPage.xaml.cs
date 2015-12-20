using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.ViewModels;
using Xamarin.Forms;
using AdaaMobile.Strings;

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

        }

        private async void EmployeeSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(EmployeeSearchBar.Text))
            {
                _directoryViewModel.GetGroupedEmployees();
            }
            else
            {
                _directoryViewModel.Search(EmployeeSearchBar.Text);
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
