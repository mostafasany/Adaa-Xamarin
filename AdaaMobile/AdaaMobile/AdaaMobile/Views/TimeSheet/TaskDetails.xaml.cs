using Xamarin.Forms;
using AdaaMobile.ViewModels;
using System;

namespace AdaaMobile
{
	public partial class TaskDetails : ContentPage
	{
		private readonly MyTimeSheetViewModel _viewModel;
		private ToolbarItem _addToolBarItem;

		public TaskDetails()
		{
			InitializeComponent();
			NavigationPage.SetBackButtonTitle(this, "");
			_viewModel = Locator.Default.MyTimeSheetViewModel;
			BindingContext = _viewModel.SelectedProjectTask;
			if (!_viewModel.SelectedProjectTask.CanEdit)
			{
				string addIcon = Device.OnPlatform("note", "note.png", "note.png");

				Action action = () =>
				{
					this.Navigation.PushAsync(new EditTask());
				};
				_addToolBarItem =
					new ToolbarItem("", addIcon, action, ToolbarItemOrder.Primary);
				ToolbarItems.Add(_addToolBarItem);
			}
			Title = _viewModel.SelectedProjectTask.Name;
		}

		private void EditTask()
		{
		}

	}
}

