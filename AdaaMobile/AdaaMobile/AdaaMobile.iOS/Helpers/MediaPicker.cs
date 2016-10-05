// ***********************************************************************
// Assembly         : XLabs.Platform.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="MediaPicker.cs" company="XLabs Team">
//     Copyright (c) XLabs Team. All rights reserved.
// </copyright>
// <summary>
//       This project is licensed under the Apache 2.0 license
//       https://github.com/XLabs/Xamarin-Forms-Labs/blob/master/LICENSE
//       
//       XLabs is a open source project that aims to provide a powerfull and cross 
//       platform set of controls tailored to work with Xamarin Forms.
// </summary>
// ***********************************************************************
// 

using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms;
<<<<<<< HEAD
using Xamarin.Forms.Labs.Services.Media;
using XLabs.Platform.Services.Media;
=======
<<<<<<< HEAD
=======
using AdaaMobile.iOS.Helpers;
>>>>>>> origin/master

>>>>>>> origin/master

[assembly: Dependency(typeof(MediaPicker))]
namespace XLabs.Platform.Services.Media
{
	/// <summary>
	/// Class MediaPicker.
	/// </summary>
	public class MediaPicker : IMediaPicker
	{
		/// <summary>
		/// The type image
		/// </summary>
		internal const string TypeImage = "public.image";

		/// <summary>
		/// The type movie
		/// </summary>
		internal const string TypeMovie = "public.movie";

		/// <summary>
		/// The _picker delegate
		/// </summary>
		private UIImagePickerControllerDelegate _pickerDelegate;

		/// <summary>
		/// The _popover
		/// </summary>
		private UIPopoverController _popover;

		/// <summary>
		/// Initializes a new instance of the <see cref="MediaPicker"/> class.
		/// </summary>
		public MediaPicker()
		{
			IsCameraAvailable = UIImagePickerController.IsSourceTypeAvailable(UIImagePickerControllerSourceType.Camera);

			var availableCameraMedia = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.Camera)
									   ?? new string[0];
			var availableLibraryMedia =
				UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.PhotoLibrary) ?? new string[0];

			foreach (var type in availableCameraMedia.Concat(availableLibraryMedia))
			{
				if (type == TypeMovie)
				{
					IsVideosSupported = true;
				}
				else if (type == TypeImage)
				{
					IsPhotosSupported = true;
				}
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance is camera available.
		/// </summary>
		/// <value><c>true</c> if this instance is camera available; otherwise, <c>false</c>.</value>
		public bool IsCameraAvailable { get; private set; }

		/// <summary>
		/// Gets a value indicating whether this instance is photos supported.
		/// </summary>
		/// <value><c>true</c> if this instance is photos supported; otherwise, <c>false</c>.</value>
		public bool IsPhotosSupported { get; private set; }

		/// <summary>
		/// Gets a value indicating whether this instance is videos supported.
		/// </summary>
		/// <value><c>true</c> if this instance is videos supported; otherwise, <c>false</c>.</value>
		public bool IsVideosSupported { get; private set; }

		/// <summary>
		/// Event the fires when media has been selected
		/// </summary>
		/// <value>The on photo selected.</value>
		public EventHandler<MediaPickerArgs> OnMediaSelected { get; set; }

		/// <summary>
		/// Gets or sets the on error.
		/// </summary>
		/// <value>The on error.</value>
		public EventHandler<MediaPickerErrorArgs> OnError { get; set; }

		/// <summary>
		/// Select a picture from library.
		/// </summary>
		/// <param name="options">The storage options.</param>
		/// <returns>Task&lt;IMediaFile&gt;.</returns>
		/// <exception cref="NotSupportedException"></exception>
		public Task<MediaFile> SelectPhotoAsync(CameraMediaStorageOptions options)
		{
			if (!IsPhotosSupported)
			{
				throw new NotSupportedException();
			}

			return GetMediaAsync(UIImagePickerControllerSourceType.PhotoLibrary, TypeImage);
		}

		/// <summary>
		/// Takes the picture.
		/// </summary>
		/// <param name="options">The storage options.</param>
		/// <returns>Task&lt;IMediaFile&gt;.</returns>
		/// <exception cref="NotSupportedException">
		/// </exception>
		public Task<MediaFile> TakePhotoAsync(CameraMediaStorageOptions options)
		{
			if (!IsPhotosSupported)
			{
				throw new NotSupportedException();
			}
			if (!IsCameraAvailable)
			{
				throw new NotSupportedException();
			}

			VerifyCameraOptions(options);

			return GetMediaAsync(UIImagePickerControllerSourceType.Camera, TypeImage, options);
		}

		/// <summary>
		/// Selects the video asynchronous.
		/// </summary>
		/// <param name="options">The options.</param>
		/// <returns>Task&lt;IMediaFile&gt;.</returns>
		/// <exception cref="NotSupportedException"></exception>
		public Task<MediaFile> SelectVideoAsync(VideoMediaStorageOptions options)
		{
			if (!IsPhotosSupported)
			{
				throw new NotSupportedException();
			}

			return GetMediaAsync(UIImagePickerControllerSourceType.PhotoLibrary, TypeMovie);
		}

		/// <summary>
		/// Takes the video asynchronous.
		/// </summary>
		/// <param name="options">The options.</param>
		/// <returns>Task&lt;IMediaFile&gt;.</returns>
		/// <exception cref="NotSupportedException">
		/// </exception>
		public Task<MediaFile> TakeVideoAsync(VideoMediaStorageOptions options)
		{
			if (!IsVideosSupported)
			{
				throw new NotSupportedException();
			}
			if (!IsCameraAvailable)
			{
				throw new NotSupportedException();
			}

			//VerifyCameraOptions (options);

			return GetMediaAsync(UIImagePickerControllerSourceType.Camera, TypeMovie, options);
		}

		/// <summary>
		/// Gets the media asynchronous.
		/// </summary>
		/// <param name="sourceType">Type of the source.</param>
		/// <param name="mediaType">Type of the media.</param>
		/// <param name="options">The options.</param>
		/// <returns>Task&lt;MediaFile&gt;.</returns>
		/// <exception cref="InvalidOperationException">
		/// There's no current active window
		/// or
		/// Could not find current view controller
		/// or
		/// Only one operation can be active at at time
		/// </exception>
		private Task<MediaFile> GetMediaAsync(
			UIImagePickerControllerSourceType sourceType,
			string mediaType,
			MediaStorageOptions options = null)
		{
			var window = UIApplication.SharedApplication.KeyWindow;
			if (window == null)
			{
				throw new InvalidOperationException("There's no current active window");
			}

			var viewController = window.RootViewController;

#if __IOS_10__
            if (viewController == null || (viewController.PresentedViewController != null && viewController.PresentedViewController.GetType() == typeof(UIAlertController)))
            {
                window =
                    UIApplication.SharedApplication.Windows.OrderByDescending(w => w.WindowLevel)
                        .FirstOrDefault(w => w.RootViewController != null);

                if (window == null)
                {
                    throw new InvalidOperationException("Could not find current view controller");
                }

                viewController = window.RootViewController;
            }
#endif
			while (viewController.PresentedViewController != null)
			{
				viewController = viewController.PresentedViewController;
			}

			var ndelegate = new MediaPickerDelegate(viewController, sourceType, options);
			var od = Interlocked.CompareExchange(ref _pickerDelegate, ndelegate, null);
			if (od != null)
			{
				throw new InvalidOperationException("Only one operation can be active at at time");
			}

			var picker = SetupController(ndelegate, sourceType, mediaType, options);

			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad
				&& sourceType == UIImagePickerControllerSourceType.PhotoLibrary)
			{
				ndelegate.Popover = new UIPopoverController(picker)
				{
					Delegate = new MediaPickerPopoverDelegate(ndelegate, picker)
				};
				ndelegate.DisplayPopover();
			}
			else
			{
				viewController.PresentViewController(picker, true, null);
			}

			return ndelegate.Task.ContinueWith(
				t =>
					{
						if (_popover != null)
						{
							_popover.Dispose();
							_popover = null;
						}

						Interlocked.Exchange(ref _pickerDelegate, null);
						return t;
					}).Unwrap();
		}

		/// <summary>
		/// Setups the controller.
		/// </summary>
		/// <param name="mpDelegate">The mp delegate.</param>
		/// <param name="sourceType">Type of the source.</param>
		/// <param name="mediaType">Type of the media.</param>
		/// <param name="options">The options.</param>
		/// <returns>MediaPickerController.</returns>
		private static MediaPickerController SetupController(
			MediaPickerDelegate mpDelegate,
			UIImagePickerControllerSourceType sourceType,
			string mediaType,
			MediaStorageOptions options = null)
		{
			var picker = new MediaPickerController(mpDelegate) { MediaTypes = new[] { mediaType }, SourceType = sourceType };

			if (sourceType == UIImagePickerControllerSourceType.Camera)
			{
				if (mediaType == TypeImage && options is CameraMediaStorageOptions)
				{
					picker.CameraDevice = GetCameraDevice(((CameraMediaStorageOptions)options).DefaultCamera);
					picker.CameraCaptureMode = UIImagePickerControllerCameraCaptureMode.Photo;
				}
				else if (mediaType == TypeMovie && options is VideoMediaStorageOptions)
				{
					var voptions = (VideoMediaStorageOptions)options;

					picker.CameraDevice = GetCameraDevice(voptions.DefaultCamera);
					picker.CameraCaptureMode = UIImagePickerControllerCameraCaptureMode.Video;
					picker.VideoQuality = GetQuailty(voptions.Quality);
					picker.VideoMaximumDuration = voptions.DesiredLength.TotalSeconds;
				}
			}

			return picker;
		}

		/// <summary>
		/// Gets the UI camera device.
		/// </summary>
		/// <param name="device">The device.</param>
		/// <returns>UIImagePickerControllerCameraDevice.</returns>
		/// <exception cref="NotSupportedException"></exception>
		private static UIImagePickerControllerCameraDevice GetCameraDevice(CameraDevice device)
		{
			switch (device)
			{
				case CameraDevice.Front:
					return UIImagePickerControllerCameraDevice.Front;
				case CameraDevice.Rear:
					return UIImagePickerControllerCameraDevice.Rear;
				default:
					throw new NotSupportedException();
			}
		}

		/// <summary>
		/// Gets the quailty.
		/// </summary>
		/// <param name="quality">The quality.</param>
		/// <returns>UIImagePickerControllerQualityType.</returns>
		private static UIImagePickerControllerQualityType GetQuailty(VideoQuality quality)
		{
			switch (quality)
			{
				case VideoQuality.Low:
					return UIImagePickerControllerQualityType.Low;
				case VideoQuality.Medium:
					return UIImagePickerControllerQualityType.Medium;
				default:
					return UIImagePickerControllerQualityType.High;
			}
		}

		/// <summary>
		/// Verifies the options.
		/// </summary>
		/// <param name="options">The options.</param>
		/// <exception cref="ArgumentNullException">options</exception>
		/// <exception cref="ArgumentException">options.Directory must be a relative path;options</exception>
		private static void VerifyOptions(MediaStorageOptions options)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			if (options.Directory != null && Path.IsPathRooted(options.Directory))
			{
				throw new ArgumentException("options.Directory must be a relative path", "options");
			}
		}

		/// <summary>
		/// Verifies the camera options.
		/// </summary>
		/// <param name="options">The options.</param>
		/// <exception cref="ArgumentException">options.Camera is not a member of CameraDevice</exception>
		private static void VerifyCameraOptions(CameraMediaStorageOptions options)
		{
			VerifyOptions(options);
			if (!Enum.IsDefined(typeof(CameraDevice), options.DefaultCamera))
			{
				throw new ArgumentException("options.Camera is not a member of CameraDevice");
			}
		}
	}

	internal class MediaPickerDelegate : UIImagePickerControllerDelegate
	{
		/// <summary>
		/// The _orientation
		/// </summary>
		private UIDeviceOrientation? _orientation;

		/// <summary>
		/// The _observer
		/// </summary>
		private readonly NSObject _observer;

		/// <summary>
		/// The _options
		/// </summary>
		private readonly MediaStorageOptions _options;

		/// <summary>
		/// The _source
		/// </summary>
		private readonly UIImagePickerControllerSourceType _source;

		/// <summary>
		/// The _TCS
		/// </summary>
		private readonly TaskCompletionSource<MediaFile> _tcs = new TaskCompletionSource<MediaFile>();

		/// <summary>
		/// The _view controller
		/// </summary>
		private readonly UIViewController _viewController;

		/// <summary>
		/// Initializes a new instance of the <see cref="MediaPickerDelegate"/> class.
		/// </summary>
		/// <param name="viewController">The view controller.</param>
		/// <param name="sourceType">Type of the source.</param>
		/// <param name="options">The options.</param>
		internal MediaPickerDelegate(
			UIViewController viewController,
			UIImagePickerControllerSourceType sourceType,
			MediaStorageOptions options)
		{
			_viewController = viewController;
			_source = sourceType;
			_options = options ?? new CameraMediaStorageOptions();

			if (viewController != null)
			{
				UIDevice.CurrentDevice.BeginGeneratingDeviceOrientationNotifications();
				_observer = NSNotificationCenter.DefaultCenter.AddObserver(UIDevice.OrientationDidChangeNotification, DidRotate);
			}
		}

		/// <summary>
		/// Gets or sets the popover.
		/// </summary>
		/// <value>The popover.</value>
		public UIPopoverController Popover { get; set; }

		/// <summary>
		/// Gets the view.
		/// </summary>
		/// <value>The view.</value>
		public UIView View
		{
			get
			{
				return _viewController.View;
			}
		}

		/// <summary>
		/// Gets the task.
		/// </summary>
		/// <value>The task.</value>
		public Task<MediaFile> Task
		{
			get
			{
				return _tcs.Task;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance is captured.
		/// </summary>
		/// <value><c>true</c> if this instance is captured; otherwise, <c>false</c>.</value>
		private bool IsCaptured
		{
			get
			{
				return _source == UIImagePickerControllerSourceType.Camera;
			}
		}

		/// <summary>
		/// Finisheds the picking media.
		/// </summary>
		/// <param name="picker">The picker.</param>
		/// <param name="info">The information.</param>
		/// <exception cref="NotSupportedException"></exception>
		public override void FinishedPickingMedia(UIImagePickerController picker, NSDictionary info)
		{
			MediaFile mediaFile;
			switch ((NSString)info[UIImagePickerController.MediaType])
			{
				case MediaPicker.TypeImage:
					mediaFile = GetPictureMediaFile(info);
					break;

				case MediaPicker.TypeMovie:
					mediaFile = GetMovieMediaFile(info);
					break;

				default:
					throw new NotSupportedException();
			}

			Dismiss(picker, () => _tcs.TrySetResult(mediaFile));
		}

		/// <summary>
		/// Canceleds the specified picker.
		/// </summary>
		/// <param name="picker">The picker.</param>
		public override void Canceled(UIImagePickerController picker)
		{
			Dismiss(picker, () => _tcs.TrySetCanceled());
		}

		/// <summary>
		/// Displays the popover.
		/// </summary>
		/// <param name="hideFirst">if set to <c>true</c> [hide first].</param>
		public void DisplayPopover(bool hideFirst = false)
		{
			if (Popover == null)
			{
				return;
			}

			var swidth = UIScreen.MainScreen.Bounds.Width;
			var sheight = UIScreen.MainScreen.Bounds.Height;

			float width = 400;
			float height = 300;

			if (_orientation == null)
			{
				if (IsValidInterfaceOrientation(UIDevice.CurrentDevice.Orientation))
				{
					_orientation = UIDevice.CurrentDevice.Orientation;
				}
				else
				{
					_orientation = GetDeviceOrientation(_viewController.InterfaceOrientation);
				}
			}

			float x, y;
			if (_orientation == UIDeviceOrientation.LandscapeLeft || _orientation == UIDeviceOrientation.LandscapeRight)
			{
				y = (float)(swidth / 2) - (height / 2);
				x = (float)(sheight / 2) - (width / 2);
			}
			else
			{
				x = (float)(swidth / 2) - (width / 2);
				y = (float)(sheight / 2) - (height / 2);
			}

			if (hideFirst && Popover.PopoverVisible)
			{
				Popover.Dismiss(false);
			}

			Popover.PresentFromRect(new CGRect(x, y, width, height), View, 0, true);
		}

		/// <summary>
		/// Dismisses the specified picker.
		/// </summary>
		/// <param name="picker">The picker.</param>
		/// <param name="onDismiss">The on dismiss.</param>
		private void Dismiss(UIImagePickerController picker, Action onDismiss)
		{
			if (_viewController == null)
			{
				onDismiss();
			}
			else
			{
				NSNotificationCenter.DefaultCenter.RemoveObserver(_observer);
				UIDevice.CurrentDevice.EndGeneratingDeviceOrientationNotifications();

				_observer.Dispose();

				if (Popover != null)
				{
					Popover.Dismiss(true);
					Popover.Dispose();
					Popover = null;

					onDismiss();
				}
				else
				{
					picker.DismissViewController(true, onDismiss);
					picker.Dispose();
				}
			}
		}

		/// <summary>
		/// Dids the rotate.
		/// </summary>
		/// <param name="notice">The notice.</param>
		private void DidRotate(NSNotification notice)
		{
			var device = (UIDevice)notice.Object;
			if (!IsValidInterfaceOrientation(device.Orientation) || Popover == null)
			{
				return;
			}
			if (_orientation.HasValue && IsSameOrientationKind(_orientation.Value, device.Orientation))
			{
				return;
			}

			if (UIDevice.CurrentDevice.CheckSystemVersion(6, 0))
			{
				if (!GetShouldRotate6(device.Orientation))
				{
					return;
				}
<<<<<<< HEAD
			}
			else if (!GetShouldRotate(device.Orientation))
			{
				return;
			}

			var co = _orientation;
			_orientation = device.Orientation;

			if (co == null)
			{
				return;
			}

			DisplayPopover(true);
		}

		/// <summary>
		/// Gets the should rotate.
		/// </summary>
		/// <param name="orientation">The orientation.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		private bool GetShouldRotate(UIDeviceOrientation orientation)
		{
			var iorientation = UIInterfaceOrientation.Portrait;
			switch (orientation)
			{
				case UIDeviceOrientation.LandscapeLeft:
					iorientation = UIInterfaceOrientation.LandscapeLeft;
					break;

				case UIDeviceOrientation.LandscapeRight:
					iorientation = UIInterfaceOrientation.LandscapeRight;
					break;

				case UIDeviceOrientation.Portrait:
					iorientation = UIInterfaceOrientation.Portrait;
					break;

				case UIDeviceOrientation.PortraitUpsideDown:
					iorientation = UIInterfaceOrientation.PortraitUpsideDown;
					break;

				default:
					return false;
			}

			return _viewController.ShouldAutorotateToInterfaceOrientation(iorientation);
		}

		/// <summary>
		/// Gets the should rotate6.
		/// </summary>
		/// <param name="orientation">The orientation.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		private bool GetShouldRotate6(UIDeviceOrientation orientation)
		{
			if (!_viewController.ShouldAutorotate())
			{
				return false;
			}

			var mask = UIInterfaceOrientationMask.Portrait;
			switch (orientation)
			{
				case UIDeviceOrientation.LandscapeLeft:
					mask = UIInterfaceOrientationMask.LandscapeLeft;
					break;

				case UIDeviceOrientation.LandscapeRight:
					mask = UIInterfaceOrientationMask.LandscapeRight;
					break;

				case UIDeviceOrientation.Portrait:
					mask = UIInterfaceOrientationMask.Portrait;
					break;

				case UIDeviceOrientation.PortraitUpsideDown:
					mask = UIInterfaceOrientationMask.PortraitUpsideDown;
					break;

				default:
					return false;
			}

			return _viewController.GetSupportedInterfaceOrientations().HasFlag(mask);
		}

		/// <summary>
		/// Gets the picture media file.
		/// </summary>
		/// <param name="info">The information.</param>
		/// <returns>MediaFile.</returns>
		private MediaFile GetPictureMediaFile(NSDictionary info)
		{
			var image = (UIImage)info[UIImagePickerController.EditedImage];
			if (image == null)
			{
				image = (UIImage)info[UIImagePickerController.OriginalImage];
			}

			var path = GetOutputPath(
				MediaPicker.TypeImage,
				_options.Directory ?? ((IsCaptured) ? String.Empty : "temp"),
				_options.Name);

			using (var fs = File.OpenWrite(path))
			using (Stream s = new NsDataStream(image.AsJPEG()))
			{
				s.CopyTo(fs);
				fs.Flush();
			}

			Action<bool> dispose = null;
			if (_source != UIImagePickerControllerSourceType.Camera)
			{
				dispose = d => File.Delete(path);
			}

			return new MediaFile(path, () => File.OpenRead(path), dispose);
		}

		/// <summary>
		/// Gets the movie media file.
		/// </summary>
		/// <param name="info">The information.</param>
		/// <returns>MediaFile.</returns>
		private MediaFile GetMovieMediaFile(NSDictionary info)
		{
			var url = (NSUrl)info[UIImagePickerController.MediaURL];

			var path = GetOutputPath(
				MediaPicker.TypeMovie,
				_options.Directory ?? ((IsCaptured) ? String.Empty : "temp"),
				_options.Name ?? Path.GetFileName(url.Path));

			File.Move(url.Path, path);

			Action<bool> dispose = null;
			if (_source != UIImagePickerControllerSourceType.Camera)
			{
				dispose = d => File.Delete(path);
			}

			return new MediaFile(path, () => File.OpenRead(path), dispose);
		}

		/// <summary>
		/// Gets the unique path.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="path">The path.</param>
		/// <param name="name">The name.</param>
		/// <returns>System.String.</returns>
		private static string GetUniquePath(string type, string path, string name)
		{
			var isPhoto = (type == MediaPicker.TypeImage);
			var ext = Path.GetExtension(name);
			if (ext == String.Empty)
			{
				ext = ((isPhoto) ? ".jpg" : ".mp4");
			}
=======
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
>>>>>>> origin/master

			name = Path.GetFileNameWithoutExtension(name);

			var nname = name + ext;
			var i = 1;
			while (File.Exists(Path.Combine(path, nname)))
			{
				nname = name + "_" + (i++) + ext;
			}

			return Path.Combine(path, nname);
		}

		/// <summary>
		/// Gets the output path.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="path">The path.</param>
		/// <param name="name">The name.</param>
		/// <returns>System.String.</returns>
		private static string GetOutputPath(string type, string path, string name)
		{
			path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), path);
			Directory.CreateDirectory(path);

			if (String.IsNullOrWhiteSpace(name))
			{
				var timestamp = DateTime.Now.ToString("yyyMMdd_HHmmss");
				if (type == MediaPicker.TypeImage)
				{
					name = "IMG_" + timestamp + ".jpg";
				}
				else
				{
					name = "VID_" + timestamp + ".mp4";
				}
			}

			return Path.Combine(path, GetUniquePath(type, path, name));
		}

		/// <summary>
		/// Determines whether [is valid interface orientation] [the specified self].
		/// </summary>
		/// <param name="self">The self.</param>
		/// <returns><c>true</c> if [is valid interface orientation] [the specified self]; otherwise, <c>false</c>.</returns>
		private static bool IsValidInterfaceOrientation(UIDeviceOrientation self)
		{
			return (self != UIDeviceOrientation.FaceUp && self != UIDeviceOrientation.FaceDown
					&& self != UIDeviceOrientation.Unknown);
		}

		/// <summary>
		/// Determines whether [is same orientation kind] [the specified o1].
		/// </summary>
		/// <param name="o1">The o1.</param>
		/// <param name="o2">The o2.</param>
		/// <returns><c>true</c> if [is same orientation kind] [the specified o1]; otherwise, <c>false</c>.</returns>
		private static bool IsSameOrientationKind(UIDeviceOrientation o1, UIDeviceOrientation o2)
		{
			if (o1 == UIDeviceOrientation.FaceDown || o1 == UIDeviceOrientation.FaceUp)
			{
				return (o2 == UIDeviceOrientation.FaceDown || o2 == UIDeviceOrientation.FaceUp);
			}
			if (o1 == UIDeviceOrientation.LandscapeLeft || o1 == UIDeviceOrientation.LandscapeRight)
			{
				return (o2 == UIDeviceOrientation.LandscapeLeft || o2 == UIDeviceOrientation.LandscapeRight);
			}
			if (o1 == UIDeviceOrientation.Portrait || o1 == UIDeviceOrientation.PortraitUpsideDown)
			{
				return (o2 == UIDeviceOrientation.Portrait || o2 == UIDeviceOrientation.PortraitUpsideDown);
			}

			return false;
		}

		/// <summary>
		/// Gets the device orientation.
		/// </summary>
		/// <param name="self">The self.</param>
		/// <returns>UIDeviceOrientation.</returns>
		/// <exception cref="InvalidOperationException"></exception>
		private static UIDeviceOrientation GetDeviceOrientation(UIInterfaceOrientation self)
		{
			switch (self)
			{
				case UIInterfaceOrientation.LandscapeLeft:
					return UIDeviceOrientation.LandscapeLeft;
				case UIInterfaceOrientation.LandscapeRight:
					return UIDeviceOrientation.LandscapeRight;
				case UIInterfaceOrientation.Portrait:
					return UIDeviceOrientation.Portrait;
				case UIInterfaceOrientation.PortraitUpsideDown:
					return UIDeviceOrientation.PortraitUpsideDown;
				default:
					throw new InvalidOperationException();
			}
		}
	}
	/// <summary>
	/// Class MediaPickerPopoverDelegate.
	/// </summary>
	internal class MediaPickerPopoverDelegate : UIPopoverControllerDelegate
	{
		/// <summary>
		/// The _picker
		/// </summary>
		private readonly UIImagePickerController _picker;

		/// <summary>
		/// The _picker delegate
		/// </summary>
		private readonly MediaPickerDelegate _pickerDelegate;

		/// <summary>
		/// Initializes a new instance of the <see cref="MediaPickerPopoverDelegate"/> class.
		/// </summary>
		/// <param name="pickerDelegate">The picker delegate.</param>
		/// <param name="picker">The picker.</param>
		internal MediaPickerPopoverDelegate(MediaPickerDelegate pickerDelegate, UIImagePickerController picker)
		{
			_pickerDelegate = pickerDelegate;
			_picker = picker;
		}

		/// <summary>
		/// Shoulds the dismiss.
		/// </summary>
		/// <param name="popoverController">The popover controller.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		public override bool ShouldDismiss(UIPopoverController popoverController)
		{
			return true;
		}

		/// <summary>
		/// Dids the dismiss.
		/// </summary>
		/// <param name="popoverController">The popover controller.</param>
		public override void DidDismiss(UIPopoverController popoverController)
		{
			_pickerDelegate.Canceled(_picker);
		}
	}

	/// <summary>
	/// Class MediaPickerController. This class cannot be inherited.
	/// </summary>
	public sealed class MediaPickerController : UIImagePickerController
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MediaPickerController"/> class.
		/// </summary>
		/// <param name="mpDelegate">The mp delegate.</param>
		internal MediaPickerController(MediaPickerDelegate mpDelegate)
		{
			base.Delegate = mpDelegate;
		}

		/// <summary>
		/// Gets or sets the delegate.
		/// </summary>
		/// <value>The delegate.</value>
		/// <exception cref="NotSupportedException"></exception>
		public override NSObject Delegate
		{
			get
			{
				return base.Delegate;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		/// <summary>
		/// Gets the result asynchronous.
		/// </summary>
		/// <returns>Task&lt;MediaFile&gt;.</returns>
		public Task<MediaFile> GetResultAsync()
		{
			return ((MediaPickerDelegate)Delegate).Task;
		}
	}

	/// <summary>
	///     Class NsDataStream.
	/// </summary>
	internal unsafe class NsDataStream : UnmanagedMemoryStream
	{
		/// <summary>
		///     The _data
		/// </summary>
		private readonly NSData _data;

		/// <summary>
		///     Initializes a new instance of the <see cref="NsDataStream" /> class.
		/// </summary>
		/// <param name="data">The data.</param>
		public NsDataStream(NSData data)
			: base((byte*)data.Bytes, (long)data.Length)
		{
			_data = data;
		}

		/// <summary>
		///     Releases the unmanaged resources used by the <see cref="T:System.IO.UnmanagedMemoryStream" /> and optionally
		///     releases the managed resources.
		/// </summary>
		/// <param name="disposing">
		///     true to release both managed and unmanaged resources; false to release only unmanaged
		///     resources.
		/// </param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_data.Dispose();
			}

			base.Dispose(disposing);
		}
	}
}