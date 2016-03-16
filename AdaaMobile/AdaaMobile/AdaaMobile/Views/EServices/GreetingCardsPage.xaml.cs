using System;
using System.Collections.Generic;

using Xamarin.Forms;
using AdaaMobile.ViewModels;
using AdaaMobile.Strings;

namespace AdaaMobile
{
	public partial class GreetingCardsPage : ContentPage
	{
		GreetingCardsViewModel _viewModel;


		public GreetingCardsPage ()
		{
			InitializeComponent ();
			NavigationPage.SetBackButtonTitle (this, "");

			_viewModel = Locator.Default.GreetingCardsViewModel;
			BindingContext = _viewModel;

			Title = AppResources.GreetingCard;
		}

		protected async override void OnAppearing ()
		{
			base.OnAppearing ();
			await _viewModel.GetCards();
			if(_viewModel.CardsList != null && _viewModel.CardsList.Count >0){
				foreach (var item in _viewModel.CardsList) {
					SourcePicker.Items.Add(item.Title);
				}
			}
		}

		private void SourceText_Tapped(object sender, EventArgs e)
		{
			SourcePicker.Unfocus();
			SourcePicker.Focus();
		}
	}
}

