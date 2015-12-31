using AdaaMobile.iOS.CustomRenderers;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TimePicker), typeof(CustomTimePickerRenderer))]

namespace AdaaMobile.iOS.CustomRenderers
{
   public class CustomTimePickerRenderer: TimePickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<TimePicker> e)
        {
            base.OnElementChanged(e);
			Control.Layer.BorderColor = UIColor.White.CGColor;
			Control.Layer.BorderWidth = 0;
			((UIDatePicker)(Control.InputView)).Layer.BorderWidth = 0;
        }
    }
}
