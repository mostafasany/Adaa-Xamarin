using Xamarin.Forms;
using AdaaMobile.ViewModels;
using AdaaMobile.Strings;
namespace AdaaMobile
{

    public partial class MyTimesheetPage : ContentPage
    {
        private readonly MyTimeSheetViewModel _viewModel;
        public MyTimesheetPage()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");

            _viewModel = Locator.Default.MyTimeSheetViewModel;
            BindingContext = _viewModel;

            Title = AppResources.TimeSheet_MyTimeSheet;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.PageLoadedCommand.Execute(null);
        }
    }
}

