using AdaaMobile.Strings;
using AdaaMobile.ViewModels;
using Xamarin.Forms;

namespace AdaaMobile.Views
{
    public partial class MyPendingTasks : ContentPage
    {
        MyPendingTasksViewModel _viewModel;
        private ToolbarItem _addToolBarItem;

        public MyPendingTasks()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");
            _viewModel = Locator.Default.MyPendingTasksViewModel;
            BindingContext = _viewModel;
            Title = AppResources.TimeSheet_MyPendingTasks;

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.PageLoadedCommand.Execute(null);
        }
    }
}
