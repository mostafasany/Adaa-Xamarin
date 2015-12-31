using System;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using AdaaMobile.iOS;


[assembly: ExportRenderer (typeof(Page), typeof(NoBackButtonPageRenderer))]

namespace AdaaMobile.iOS
{
	public class NoBackButtonPageRenderer:PageRenderer
	{
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			NavigationItem.BackBarButtonItem = new UIKit.UIBarButtonItem ("", UIKit.UIBarButtonItemStyle.Plain, null);

			//ViewController.ParentViewController.NavigationItem.SetHidesBackButton (true, false);
			//ViewController.ParentViewController.NavigationItem.BackBarButtonItem = new UIKit.UIBarButtonItem("", UIKit.UIBarButtonItemStyle.Plain, null);

		}
//
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
//			NavigationItem.BackBarButtonItem = new UIKit.UIBarButtonItem ("", UIKit.UIBarButtonItemStyle.Plain, null);
//			if(ViewController.ParentViewController != null)
//			ViewController.ParentViewController.NavigationItem.BackBarButtonItem = new UIKit.UIBarButtonItem("", UIKit.UIBarButtonItemStyle.Plain, null);
//
		}


	}
}
