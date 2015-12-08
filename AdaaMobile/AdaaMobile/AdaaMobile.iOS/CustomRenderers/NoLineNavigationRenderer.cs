using System;
using Xamarin.Forms;
using AdaaMobile.iOS;
using Xamarin.Forms.Platform.iOS;
using UIKit;


[assembly: ExportRenderer(typeof(NavigationPage), typeof(NoLineNavigationRenderer))] 
namespace AdaaMobile.iOS
{
	public class NoLineNavigationRenderer : NavigationRenderer {

		public override void ViewDidLoad(){
			base.ViewDidLoad();
			// remove lower border and shadow of the navigation bar
			NavigationBar.SetBackgroundImage(new UIImage(), UIBarMetrics.Default);
			NavigationBar.ShadowImage = new UIImage ();
		}
	}
}