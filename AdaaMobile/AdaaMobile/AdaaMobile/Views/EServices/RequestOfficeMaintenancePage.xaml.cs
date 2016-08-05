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

            //Add submit action
            Action action = () =>
            {
                _viewModel.SubmitRequestCommand.Execute(null);
            };
            ToolbarItems.Add(
                new ToolbarItem("", "right.png", action, ToolbarItemOrder.Primary));

			HandleArabicLanguageFlowDirection ();

        }


		void HandleArabicLanguageFlowDirection()
		{
			if (Locator.Default.AppSettings.SelectedCultureName.Contains("ar"))
			{
				//Service Details
				lblServiceDetails.HorizontalOptions = LayoutOptions.End;

				//Additional Comments
				lblAdditionalComments.HorizontalOptions = LayoutOptions.End;

			}
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (!_viewModel.IsLoaded)
                _viewModel.LoadFieldsCommand.Execute(null);

        }
    }
}

