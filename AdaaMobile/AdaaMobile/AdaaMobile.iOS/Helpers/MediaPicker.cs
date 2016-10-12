using System;
using System.Threading;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using Xamarin.Forms;
using XLabs.Platform.Services.Media;
using AdaaMobile.Models;
using AdaaMobile.Helpers;
using AdaaMobile.iOS;
using AdaaMobile.ViewModels;

[assembly: Dependency(typeof(MediaPicker))]
namespace XLabs.Platform.Services.Media
{
    public class MediaPicker : AdaaMobile.Helpers.IMediaPicker
    {
        AppDelegate appDelegate = UIApplication.SharedApplication.Delegate as AppDelegate;
        UIImage originalImage;
        string imageNAme;
        PPMediaFile result;
        UIImagePickerController imagePicker;
        public MediaPicker()
        {

        }

        private TaskCompletionSource<PPMediaFile> _completionSource;

        public bool IsCameraAvailable { get; set; }

        public bool IsPhotosSupported { get; set; }

        public bool IsVideosSupported { get; set; }

        public EventHandler<MediaPickerArgs> OnMediaSelected { get; set; }


        public EventHandler<MediaPickerErrorArgs> OnError { get; set; }

        protected void Handle_FinishedPickingMedia(object sender, UIImagePickerMediaPickedEventArgs e)
        {

            var tcs = Interlocked.Exchange(ref _completionSource, null);
            // determine what was selected, video or image
            bool isImage = false;
            switch (e.Info[UIImagePickerController.MediaType].ToString())
            {
                case "public.image":
                    isImage = true;
                    break;
                default:
                    break;
            }



            // if it was an image, get the other image info
            if (isImage)
            {
                // get the original image
                NSUrl referenceURL = e.Info[new NSString("UIImagePickerControllerReferenceURL")] as NSUrl;
                imageNAme = (referenceURL == null) ? "image1.png" : referenceURL.ToString();
                originalImage = e.Info[UIImagePickerController.OriginalImage] as UIImage;
                // dismiss the picker

                PPMediaFile image = new PPMediaFile();
                using (NSData imageData = originalImage.AsPNG())
                {
                    Byte[] myimageByteArray = new Byte[imageData.Length];
                    System.Runtime.InteropServices.Marshal.Copy(imageData.Bytes, myimageByteArray, 0, Convert.ToInt32(imageData.Length));
					image.Size=new Size((double)originalImage.Size.Width, (double)originalImage.Size.Height);
                    image.data = myimageByteArray;


                    image.Extension = imageNAme;
                    result = image;
                    if (OnMediaSelected != null)
                    {
                        OnMediaSelected(this, new MediaPickerArgs(result));
                    }
                    tcs.SetResult(result);
                    imagePicker.DismissViewController(true, null);

                    return;
                }
            }
            else
            {
                imagePicker.DismissViewController(true, null);
                Locator.Default.DialogManager.DisplayAlert("ADAA", "Images is only supported", "Cancel");
            }

            tcs.SetResult(null);
        }
        public Task<PPMediaFile> SelectPhotoAsync()
        {
            var window = UIApplication.SharedApplication.KeyWindow;
            var viewController = window.RootViewController;
            imagePicker = new UIImagePickerController();
            imagePicker.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;
            imagePicker.MediaTypes = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.PhotoLibrary);
            imagePicker.FinishedPickingMedia += Handle_FinishedPickingMedia;
            imagePicker.Canceled += Handle_Canceled;
            imagePicker.NavigationBar.TintColor = UIColor.White;
            imagePicker.NavigationBar.TitleTextAttributes = new UIStringAttributes { ForegroundColor = UIColor.White };
            viewController.PresentViewController(imagePicker, true, () => { Console.WriteLine("Complete"); });
            var ntcs = new TaskCompletionSource<PPMediaFile>();
            if (Interlocked.CompareExchange(ref _completionSource, ntcs, null) != null)
                throw new InvalidOperationException("Only one operation can be active at at time");


            return ntcs.Task;

        }


        void Handle_Canceled(object sender, EventArgs e)
        {
            imagePicker.DismissViewController(true, null);
        }
    }
}