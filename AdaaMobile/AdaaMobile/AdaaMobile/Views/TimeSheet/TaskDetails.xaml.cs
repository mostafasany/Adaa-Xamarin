using Xamarin.Forms;
using AdaaMobile.ViewModels;

namespace AdaaMobile
{
	public partial class TaskDetails : ContentPage
	{
		private readonly MyTimeSheetViewModel _viewModel;
		public TaskDetails ()
		{
			InitializeComponent ();
			NavigationPage.SetBackButtonTitle (this, "");
			_viewModel = Locator.Default.MyTimeSheetViewModel;
			BindingContext = _viewModel.SelectedProjectTask;
			Title = _viewModel.SelectedProjectTask.Name;
		}
	}
}

