using System;
using System.Collections.Generic;

using Xamarin.Forms;
using AdaaMobile.Helpers;
using AdaaMobile.Views;
using AdaaMobile.ViewModels;

namespace AdaaMobile
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
			Locator.Default.AppSettings.SelectedCultureName = "ar";

			DependencyService.Get<ILocalize>().UpdateCultureInfo("ar");
			Navigation.PushModalAsync (new LoginPage ());
		}

		void EnglishButton_Clicked (object sender, EventArgs e)
		{
			
			Locator.Default.AppSettings.SelectedCultureName = "en-us";
			DependencyService.Get<ILocalize>().UpdateCultureInfo("en-us");

			Navigation.PushModalAsync (new LoginPage ());
		}


	}
}

