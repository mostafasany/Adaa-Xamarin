using AdaaMobile.Strings;
using AdaaMobile.ViewModels;
using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace AdaaMobile
{
    public partial class EServicesPage : ContentPage
    {
        private readonly EServicesViewModel _viewModel;

        public EServicesPage()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");

            _viewModel = Locator.Default.EServicesViewModel;
            BindingContext = _viewModel;

            Title = AppResources.EServices;
        }
    }
}

