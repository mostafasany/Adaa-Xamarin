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
	public class PhoneService : IPhoneService
    {
        /// <summary>
        /// Opens native dialog to dial the specified number.
        /// </summary>
        /// <param name="number">Number to dial.</param>
        public void DialNumber(string number)
        {
            if (string.IsNullOrEmpty(number))
                return;
            UIApplication.SharedApplication.OpenUrl(NSUrl.FromString("tel://" + number.Replace(" ", "")));
        }

        public void ComposeMail(string recipient, string subject, string messagebody = null, Action<bool> completed = null)
        {

            var controller = new MFMailComposeViewController();
            controller.SetToRecipients(new string[] { recipient });
            controller.SetSubject(subject);
            if (!string.IsNullOrEmpty(messagebody))
                controller.SetMessageBody(messagebody, false);
            controller.Finished += (object sender, MFComposeResultEventArgs e) =>
            {
                if (completed != null)
                    completed(e.Result == MFMailComposeResult.Sent);
                e.Controller.DismissViewController(true, null);
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

        public void OpenOracleApp()
        {
			//UIApplication.SharedApplication.OpenUrl(NSUrl.FromString("https://witunes.apple.com/us/app/ad-dof/id1032231693?mt=8"));


			UIApplication.SharedApplication.OpenUrl(new NSUrl(@"itms-apps://itunes.apple.com/app/id1032231693" ));
        }


		public void SavePictureToDisk (string filename, byte[] imageData)
		{
			var chartImage = new UIImage(NSData.FromArray(imageData));

			bool foundError = false;

			chartImage.SaveToPhotosAlbum((image, error) =>
				{
					//you can retrieve the saved UI Image as well if needed using
					//var i = image as UIImage;
					if(error != null)
					{
						Console.WriteLine(error.ToString());
						foundError = true;
					}
				});
		}

        public void ComposeMailWithAttachment(string recipient, string subject, byte[] imageData, string messagebody = null)
        {
            throw new NotImplementedException();
        }
    }

}

