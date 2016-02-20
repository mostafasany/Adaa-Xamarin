using System;

using AdaaMobile.Helpers;
using AdaaMobile.iOS.Helpers;
using Foundation;
using Xamarin.Forms;
using System.Threading;
using UIKit;
using MessageUI;



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

			var controller = new MFMailComposeViewController ();
			controller.SetToRecipients ( new string[]{ recipient});
			controller.SetSubject (subject);
			if (!string.IsNullOrEmpty (messagebody))
				controller.SetMessageBody (messagebody, false);
			controller.Finished += (object sender, MFComposeResultEventArgs e) => {
				if (completed != null)
					completed (e.Result == MFMailComposeResult.Sent);
				e.Controller.DismissViewController (true, null);
			};

//			//Adapt this to your app structure
//			var rootController = ((AppDelegate)(UIApplication.SharedApplication.Delegate)).Window.RootViewController.ChildViewControllers[0].ChildViewControllers[1].ChildViewControllers[0];
//			var navcontroller = rootController as UINavigationController;
//			if (navcontroller != null)
//				
//				rootController = navcontroller.VisibleViewController;
//

//			var rootController = ((AppDelegate)(UIApplication.SharedApplication.Delegate)).Window.RootViewController.PresentedViewController;
//			var navcontroller = rootController as UINavigationController;
//			if (navcontroller != null)
//				rootController = navcontroller.VisibleViewController;

			UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(controller, true, null);

			//rootController.PresentViewController (controller, true, null);

		}
	}
}

