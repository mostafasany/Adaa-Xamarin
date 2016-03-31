using System.IO;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System;
using AdaaMobile.Controls;
using AdaaMobile.iOS.CustomRenderers;

[assembly: ExportRenderer(typeof(Picker), typeof(ExtendedPickerRenderer))]

namespace AdaaMobile.iOS.CustomRenderers
{
	public class ExtendedPickerRenderer: PickerRenderer
	{
		protected override void OnElementChanged (ElementChangedEventArgs<Picker> e)
		{
			base.OnElementChanged (e);
			var x = (Control.InputView as UIPickerView).Model;
		}
	}


}

