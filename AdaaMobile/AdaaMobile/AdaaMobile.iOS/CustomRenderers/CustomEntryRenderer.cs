using System;
using Xamarin.Forms;
using AdaaMobile.iOS;
using Xamarin.Forms.Platform.iOS;
using UIKit;


[assembly: ExportRenderer (typeof(Entry), typeof(CustomEntryRenderer))]
namespace AdaaMobile.iOS
{
	public class CustomEntryRenderer:EntryRenderer
	{
		public CustomEntryRenderer ()
		{
		}

		protected override void OnElementChanged (ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged (e);

			if (Control != null) {
				Control.BorderStyle = UITextBorderStyle.None;


			}
		}
	}
}

