using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Threading.Tasks;
using AdaaMobile.Helpers;
using AdaaMobile.Views.MasterView;

namespace AdaaMobile.Views
{
	public partial class SplashPage : ContentPage
	{
		public SplashPage ()
		{
			InitializeComponent ();
			StartTimer ();
		}

		private async void StartTimer(){

			await Task.Delay(new TimeSpan(0,0,5));


			AppSettings settings = new AppSettings ();
			if (string.IsNullOrEmpty (settings.UserToken)) {
				Navigation.PushModalAsync (new ChooseLanguagePage());
			} else {
				Navigation.PushModalAsync (new MasterPage());
			}

//			Device.StartTimer (new TimeSpan(0,0,3), () => {
//				Navigation.PushModalAsync(new ChooseLanguagePage());
//				return true;
//			});
		}
	}
}

