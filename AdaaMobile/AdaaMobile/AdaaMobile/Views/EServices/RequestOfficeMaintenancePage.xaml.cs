using System;
using AdaaMobile.Strings;
using AdaaMobile.ViewModels;
using Xamarin.Forms;

namespace AdaaMobile.Views.EServices
{
    public partial class RequestOfficeMaintenancePage : ContentPage
    {
        OfficeMaintenanceViewModel _viewModel;

        public RequestOfficeMaintenancePage()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");
            Title = AppResources.RequestOfficeMaintenance;

            _viewModel = Locator.Default.OfficeMaintenanceViewModel;
            BindingContext = _viewModel;

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (!_viewModel.IsLoaded)
                _viewModel.LoadFieldsCommand.Execute(null);

        }
    }
}

