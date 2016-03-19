using AdaaMobile.Strings;
using AdaaMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace AdaaMobile.Views
{
    public partial class MyRequestsPage : ContentPage
    {
        MyRequestsViewModel _viewModel;

        public MyRequestsPage()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");

            _viewModel = Locator.Default.MyRequestsViewModel;
            BindingContext = _viewModel;

            Title = AppResources.MyRequests;
        }
    }
}
