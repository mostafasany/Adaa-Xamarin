using System;
using System.Threading.Tasks;
using System.Threading;
using UIKit;
using Foundation;
using AdaaMobile.Models;
using AdaaMobile.Helpers;
using AdaaMobile.ViewModels;
using AdaaMobile.Strings;
using Xamarin.Forms;

namespace AdaaMobile.iOS.Helpers
{
	[assembly: Dependency(typeof(MediaPicker))]
	public class MediaPicker :IMediaPicker
	{
		AppDelegate appDelegate = UIApplication.SharedApplication.Delegate as AppDelegate;
		UIImage originalImage;
		string imageNAme;
		PPMediaFile result;
		UIImagePickerController imagePicker;
		public MediaPicker ()
		{

		}

		private TaskCompletionSource<PPMediaFile> _completionSource;

		public bool IsCameraAvailable { get; set; }

		public bool IsPhotosSupported { get; set; }

		public bool IsVideosSupported { get; set; }

		public	EventHandler<MediaPickerArgs> OnMediaSelected { get; set; }


		public	EventHandler<MediaPickerErrorArgs> OnError { get; set; }

		protected void Handle_FinishedPickingMedia (object sender, UIImagePickerMediaPickedEventArgs e)
		{

			var tcs = Interlocked.Exchange(ref _completionSource, null);
			// determine what was selected, video or image
			bool isImage = false;
			switch(e.Info[UIImagePickerController.MediaType].ToString()) {
			case "public.image":
				isImage = true;
				break;
			default:
				break;
			}

		

			// if it was an image, get the other image info
			if (isImage) {
				// get the original image
				NSUrl referenceURL = e.Info [new NSString ("UIImagePickerControllerReferenceURL")] as NSUrl;
				imageNAme = (referenceURL == null) ? "image1.png" : referenceURL.ToString ();
				originalImage = e.Info [UIImagePickerController.OriginalImage] as UIImage;
				// dismiss the picker

				PPMediaFile image = new PPMediaFile ();
				using (NSData imageData = originalImage.AsPNG ()) {
					Byte[] myimageByteArray = new Byte[imageData.Length];
					System.Runtime.InteropServices.Marshal.Copy (imageData.Bytes, myimageByteArray, 0, Convert.ToInt32 (imageData.Length));

					image.data = myimageByteArray;
					image.Extension = imageNAme;
					result = image;
					if (OnMediaSelected != null) {
						OnMediaSelected (this, new MediaPickerArgs (result));
					}
					tcs.SetResult (result);
					imagePicker.DismissViewController (true, null);

					return;
				}
			} else {
				imagePicker.DismissViewController (true,null);
				Locator.Default.DialogManager.DisplayAlert(AppResources.ApplicationName ,"Not Allowed Type",AppResources.Ok);
				//iOSDialoge dialog = new iOSDialoge();
				//dialog.Alert(Messages.NotAllowedType, null, Messages.ok);		
			}

			tcs.SetResult (null);
		}
		public  Task<PPMediaFile> SelectPhotoAsync(){
			imagePicker = new UIImagePickerController ();
			imagePicker.SourceType = UIImagePickerControllerSourceType.SavedPhotosAlbum;
			imagePicker.MediaTypes = UIImagePickerController.AvailableMediaTypes (UIImagePickerControllerSourceType.PhotoLibrary);
			imagePicker.FinishedPickingMedia += Handle_FinishedPickingMedia;
			imagePicker.Canceled += Handle_Canceled;

			imagePicker.NavigationBar.TintColor = UIColor.White;
			imagePicker.NavigationBar.TitleTextAttributes =  new UIStringAttributes{ForegroundColor = UIColor.White};
			//Locator.Default.NavigationService.NavigateToPage();
			UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(imagePicker, true, null);
			//appDelegate.navigation.PresentViewController(imagePicker, true,null);
			var ntcs = new TaskCompletionSource<PPMediaFile>();
			if (Interlocked.CompareExchange(ref _completionSource, ntcs, null) != null)
				throw new InvalidOperationException("Only one operation can be active at at time");

			return null;
	
		}


		void Handle_Canceled (object sender, EventArgs e) {
			imagePicker.DismissViewController(true,null);
		}
	}
}

