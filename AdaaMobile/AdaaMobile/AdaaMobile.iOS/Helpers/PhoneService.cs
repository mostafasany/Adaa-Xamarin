using System;

using AdaaMobile.Helpers;
using AdaaMobile.iOS.Helpers;
using Foundation;
using Xamarin.Forms;
using System.Threading;
using UIKit;



[assembly: Dependency(typeof(PhoneService))]

namespace AdaaMobile.iOS.Helpers
{
	public class PhoneService: IPhoneService
	{
		/// <summary>
		/// Opens native dialog to dial the specified number.
		/// </summary>
		/// <param name="number">Number to dial.</param>
		public void DialNumber(string number)
		{
			if (string.IsNullOrEmpty (number))
				return;
			UIApplication.SharedApplication.OpenUrl(NSUrl.FromString("tel://" + number.Replace (" ", "")));
		}

		public void ComposeMail (string recipient, string subject, string messagebody = null, Action<bool> completed = null){

		}
	}
}

