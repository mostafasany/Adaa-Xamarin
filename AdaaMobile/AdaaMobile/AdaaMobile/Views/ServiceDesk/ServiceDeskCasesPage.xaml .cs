using AdaaMobile.Strings;
using AdaaMobile.ViewModels;
using Xamarin.Forms;

namespace AdaaMobile
{
    public partial class ServiceDeskCasesPage : ContentPage
    {
		ServiceDeskCasesViewModel _viewModel;
        public ServiceDeskCasesPage()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");

            _viewModel = Locator.Default.ServiceDeskCasesViewModel;
			BindingContext = _viewModel;

			Title = AppResources.ServiceDesk_MyCases;

        }

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
			_viewModel.PageLoadedCommand.Execute (null);
		}
    }
}
