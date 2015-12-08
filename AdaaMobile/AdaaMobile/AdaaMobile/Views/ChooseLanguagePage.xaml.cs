using System;
using AdaaMobile.Helpers;
using AdaaMobile.ViewModels;
using Xamarin.Forms;

namespace AdaaMobile.Views
{
	public partial class ChooseLanguagePage : ContentPage
	{
		public ChooseLanguagePage ()
		{
			InitializeComponent ();
		
			EnglishButton.Clicked += EnglishButton_Clicked;
			ArabicButton.Clicked += ArabicButton_Clicked1;;

		}

		void ArabicButton_Clicked1 (object sender, EventArgs e)
		{
			Locator.Default.AppSettings.SelectedCultureName = "ar-EG";

			DependencyService.Get<ILocalize>().UpdateCultureInfo("ar-EG");
			Navigation.PushModalAsync (new LoginPage ());
		}

		void EnglishButton_Clicked (object sender, EventArgs e)
		{
			
			Locator.Default.AppSettings.SelectedCultureName = "en-US";
			DependencyService.Get<ILocalize>().UpdateCultureInfo("en-US");

			Navigation.PushModalAsync (new LoginPage ());
		}


	}
}

