using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Threading.Tasks;

namespace AdaaMobile
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

				Navigation.PushModalAsync(new ChooseLanguagePage());

//			Device.StartTimer (new TimeSpan(0,0,3), () => {
//				Navigation.PushModalAsync(new ChooseLanguagePage());
//				return true;
//			});
		}
	}
}

