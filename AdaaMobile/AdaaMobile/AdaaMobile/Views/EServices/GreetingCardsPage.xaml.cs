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

            //Add Send via email action
            Action action = () =>
            {
                if (_viewModel.CardsList != null && _viewModel.CardsList.Count > 0) {
                    byte[] bytesArray = Convert.FromBase64String(_viewModel.CardsList[0].Image);
                    DependencyService.Get<IPhoneService>().ComposeMailWithAttachment("", _viewModel.CardsList[0].Title, "adaa_greeting_card.jpg", bytesArray);
                }
            };
            ToolbarItems.Add(
                new ToolbarItem("", "right.png", action, ToolbarItemOrder.Primary));
        }

		protected override async void OnAppearing ()
		{
			base.OnAppearing ();
			//SaveCardButton.IsEnabled = false;

			await _viewModel.GetCards();
			if(_viewModel.CardsList != null && _viewModel.CardsList.Count >0){
				foreach (var item in _viewModel.CardsList) {
					SourcePicker.Items.Add(item.Title);
				}
			}
			//SaveCardButton.IsEnabled = true;
		}

		private void SourceText_Tapped(object sender, EventArgs e)
		{
			SourcePicker.Unfocus();
			SourcePicker.Focus();
		}

		private async void SaveCard_Clicked(object sender, EventArgs e)
		{
			if (_viewModel.CardsList != null && _viewModel.CardsList.Count > 0) {
				byte[] bytesArray = Convert.FromBase64String (_viewModel.CardsList [0].Image);
                string name = _viewModel.CardsList[0].Title + System.DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".jpg";
                DependencyService.Get<IPhoneService> ().SavePictureToDisk (name, bytesArray);
                await Application.Current.MainPage.DisplayAlert("ADAA", AppResources.CardSavedMessaga, "OK");
            }
		}


	}
}

