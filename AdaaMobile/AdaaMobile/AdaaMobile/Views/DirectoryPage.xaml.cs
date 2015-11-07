using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.ViewModels;
using Xamarin.Forms;

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
        }

        protected  override async void OnAppearing()
        {
            base.OnAppearing();
            await Task.Delay(300);
            _directoryViewModel.LoadEmployeesCommand.Execute(null);
        }
    }
}
