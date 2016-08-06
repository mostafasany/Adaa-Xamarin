using AdaaMobile.Strings;
using AdaaMobile.ViewModels;
using Xamarin.Forms;

namespace AdaaMobile.Views
{
    public partial class MyAssigmnetsPage : ContentPage
    {
        MyAssigmentsViewModel _viewModel;
        public MyAssigmnetsPage()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");

            _viewModel = Locator.Default.MyAssigmentsViewModel;
            BindingContext = _viewModel;

            Title = AppResources.TimeSheet_MyAssignment;

        }

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
			_viewModel.PageLoadedCommand.Execute (null);
		}
    }
}
