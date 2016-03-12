using System;
using AdaaMobile.Strings;
using AdaaMobile.ViewModels;
using Xamarin.Forms;

namespace AdaaMobile.Views.EServices
{
	public partial class RequestDriverPage : ContentPage
	{
		RequestDriverViewModel _viewModel;

		public RequestDriverPage ()
		{
			InitializeComponent ();
			NavigationPage.SetBackButtonTitle(this, "");

			_viewModel = Locator.Default.RequestDriverViewModel;
			BindingContext = _viewModel;

			Title = AppResources.RequestDriver;

			//Initialize picker and wire events
			ReasonTypePicker.Items.Add(AppResources.Work);
			ReasonTypePicker.Items.Add(AppResources.Personal);
			ReasonTypePicker.SelectedIndexChanged += ReasonTypePicker_SelectedIndexChanged;

			//Add submit action
			Action action = () =>
			{
				_viewModel.NewDriverRequestCommand.Execute(null);
			};
			ToolbarItems.Add(
				new ToolbarItem("", "right.png", action, ToolbarItemOrder.Primary));
		}

		private void Priority_Tapped(object sender, EventArgs e)
		{
			PriorityPicker.Unfocus();
			PriorityPicker.Focus();
		}

		private void ReasonType_Tapped(object sender, EventArgs e)
		{
			ReasonTypePicker.Unfocus();
			ReasonTypePicker.Focus();
		}

		private void RequestDate_Tapped(object sender, EventArgs e)
		{
			RequestDatePicker.Unfocus();
			RequestDatePicker.Focus();
		}

		private void RequestTimeSpan_Tapped(object sender, EventArgs e)
		{
			RequestTimePicker.Unfocus();
			RequestTimePicker.Focus();
		}

		private void ReasonTypePicker_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ReasonTypePicker.SelectedIndex == 0)
			{
				_viewModel.ReasonType = "Work";
				_viewModel.LocalizedReasonType = AppResources.Work;
			}
			else
			{
				_viewModel.ReasonType = "Personal";
				_viewModel.LocalizedReasonType = AppResources.Personal;
			}
		}
	}
}

