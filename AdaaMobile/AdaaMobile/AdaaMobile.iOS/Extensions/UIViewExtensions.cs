﻿//Author https://github.com/paulpatarinski/Xamarin.Forms.Plugins/tree/master/RoundedBoxView

using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace AdaaMobile.iOS.Extensions
{
    public static class UiViewExtensions
    {
        public static void InitializeFrom(this UIView nativeControl, AdaaMobile.Controls.RoundedBoxView formsControl)
        {
            if (nativeControl == null || formsControl == null)
                return;

            nativeControl.Layer.MasksToBounds = true;
            nativeControl.Layer.CornerRadius = (float)formsControl.CornerRadius * 5;
            nativeControl.UpdateBorder(formsControl.BorderColor, formsControl.BorderThickness);
        }

        public static void UpdateFrom(this UIView nativeControl, AdaaMobile.Controls.RoundedBoxView formsControl,
          string propertyChanged)
        {
            if (nativeControl == null || formsControl == null)
                return;

            if (propertyChanged == AdaaMobile.Controls.RoundedBoxView.CornerRadiusProperty.PropertyName)
            {
                nativeControl.Layer.CornerRadius = (float)formsControl.CornerRadius;
            }

            if (propertyChanged == AdaaMobile.Controls.RoundedBoxView.BorderColorProperty.PropertyName)
            {
                nativeControl.UpdateBorder(formsControl.BorderColor, formsControl.BorderThickness);
            }

            if (propertyChanged == AdaaMobile.Controls.RoundedBoxView.BorderThicknessProperty.PropertyName)
            {
                nativeControl.UpdateBorder(formsControl.BorderColor, formsControl.BorderThickness);
            }
        }

        public static void UpdateBorder(this UIView nativeControl, Color color, int thickness)
        {
            nativeControl.Layer.BorderColor = color.ToCGColor();
            nativeControl.Layer.BorderWidth = thickness;
        }
    }
}