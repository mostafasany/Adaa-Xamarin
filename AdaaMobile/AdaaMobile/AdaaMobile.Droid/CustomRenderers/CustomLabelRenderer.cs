using System;
using System.Reflection.Emit;
using AdaaMobile.Droid;
using Android.Graphics;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;

[assembly: ExportRenderer (typeof (Xamarin.Forms.Label), typeof (CustomLabelRenderer))]

namespace AdaaMobile.Droid
{
	public class CustomLabelRenderer : LabelRenderer {
		protected override void OnElementChanged (ElementChangedEventArgs<Xamarin.Forms.Label> e) {
			base.OnElementChanged (e);
			var label = (TextView)Control; // for example
			Typeface font = Typeface.CreateFromAsset (Forms.Context.Assets, "ProximaNova-Regular.otf");
			label.Typeface = font;
		}
	}
}
}

