using System;
using System.Collections.Generic;

using Xamarin.Forms;
using AdaaMobile.ViewModels;
using AdaaMobile.Strings;
using AdaaMobile.Views.TimeSheet;
using AdaaMobile.Models.Response;

namespace AdaaMobile
{

	public partial class EditTask : ContentPage
	{
		private readonly MyTimeSheetViewModel _viewModel;

		public EditTask ()
		{
			InitializeComponent ();
			NavigationPage.SetBackButtonTitle (this, "");

			_viewModel = Locator.Default.MyTimeSheetViewModel;
			BindingContext = _viewModel;

			Title = AppResources.EServices;

			//Add submit action
			Action action = () => {
				EditSelectedTask ();
			};
			ToolbarItems.Add (
				new ToolbarItem ("", "right.png", action, ToolbarItemOrder.Primary));

			LoadDuration ();
			DurationPicker.SelectedIndexChanged += DurationPicker_SelectionIndexChanged;
		}
		protected override void OnAppearing ()
		{
			base.OnAppearing ();
			_viewModel.PageLoadedCommand.Execute (null);
		}


		private async void EditSelectedTask ()
		{
			var duration = lblDurationResult.Text;
			var comment = AdditionalCommentsEditor.Text;
//			var selectedTaskID = _viewModel.SelectedTask.Id;
		}


		private void LoadDuration ()
		{
			DurationPicker.Items.Clear ();
			for (int i = 0; i < 24; i++) {
				string value = i.ToString () + ":00";
				DurationPicker.Items.Add (value);
				value = i.ToString () + ":30";
				DurationPicker.Items.Add (value);
			}
			DurationPicker.SelectedIndexChanged += DurationPicker_SelectionIndexChanged;
		}

		private void Duration_OnClicked (object sender, EventArgs e)
		{
			DurationPicker.Unfocus ();
			DurationPicker.Focus ();
		}



		private void DurationPicker_SelectionIndexChanged (object sender, EventArgs e)
		{
			if (sender != null && sender is Picker) {
				var picker = sender as Picker;
				lblDurationResult.Text = DurationPicker.Items [picker.SelectedIndex];
			}
		}

	
	
	}
}

