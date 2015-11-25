using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using AdaaMobile.Controls;
using AdaaMobile.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly:
  ExportRenderer(typeof(AdaaMobile.Controls.ExtendedGrid), typeof(ExtendedGridRenderer))]
namespace AdaaMobile.iOS.CustomRenderers
{
    public class ExtendedGridRenderer : ViewRenderer<ExtendedGrid, UIView>
    {
        private AdaaMobile.Controls.ExtendedGrid _extendedGrid
        {
            get { return Element as AdaaMobile.Controls.ExtendedGrid; }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<ExtendedGrid> e)
        {
            base.OnElementChanged(e);
            if (Control == null || e.NewElement == null) return;
            UpdateCornerRadius();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == ExtendedGrid.CornerRadiusProperty.PropertyName)
            {
                UpdateCornerRadius();
            }


        }

        private void UpdateCornerRadius()
        {
            Control.Layer.BackgroundColor = Color.Transparent.ToCGColor();
            Control.Layer.MasksToBounds = true;
            Control.Layer.CornerRadius = (float)_extendedGrid.CornerRadius * 5;
        }
    }
}
