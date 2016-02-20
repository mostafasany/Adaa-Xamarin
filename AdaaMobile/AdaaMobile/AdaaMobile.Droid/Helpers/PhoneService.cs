
using System.Globalization;
using AdaaMobile.Droid.Helpers;
using AdaaMobile.Helpers;
using Java.Util;
using Xamarin.Forms;
using AdaaMobile.Droid;
using Android.Net;
using Android.Content;

[assembly: Dependency(typeof(PhoneService))]
namespace AdaaMobile.Droid
{
	public class PhoneService: IPhoneService
	{
		/// <summary>
		/// Opens native dialog to dial the specified number.
		/// </summary>
		/// <param name="number">Number to dial.</param>
		public void DialNumber(string number)
		{
			var instance = MainActivity.Instance;
			if (instance != null)
			{
				var uri = Android.Net.Uri.Parse ("tel:" + number);
				instance.StartActivity(new Intent(Intent.ActionDial, uri));
			}
		}
	}
}

