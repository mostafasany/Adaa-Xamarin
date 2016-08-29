using System;
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

			Action action = () =>
		  {
			  LoadYears();
		  };
			ToolbarItems.Add(
				new ToolbarItem("", "right.png", action, ToolbarItemOrder.Primary));


		}

		void LoadYears()
		{

			YearsPicker.Unfocus();
			YearsPicker.Focus();

			YearsPicker.Items.Clear();

			for (int i = DateTime.Now.Year + 1; i > DateTime.Now.Year - 50; i--)
			{
				YearsPicker.Items.Add(i.ToString());
			}
			YearsPicker.SelectedIndexChanged += YearsPicker_SelectionIndexChanged;

		}

		void YearsPicker_SelectionIndexChanged(object sender, EventArgs e)
		{
			var picker = sender as Picker;
			string year = YearsPicker.Items[picker.SelectedIndex];
			(BindingContext as MyAssigmentsViewModel).FilterByDate(year);
			YearsPicker.Unfocus();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			_viewModel.PageLoadedCommand.Execute(null);
		}
	}
}
