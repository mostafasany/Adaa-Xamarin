using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using AdaaMobile.Controls;
using AdaaMobile.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using CoreAnimation;
using CoreGraphics;
using Foundation;

[assembly:
  ExportRenderer(typeof(AdaaMobile.Controls.ExtendedGrid), typeof(ExtendedGridRenderer))]
namespace AdaaMobile.iOS.CustomRenderers
{
    [Preserve(AllMembers = true)]
    public class ExtendedGridRenderer : VisualElementRenderer<ExtendedGrid>
    {
        /// <summary>
        /// Used for registration with dependency service
        /// </summary>
        public static void Init()
        {
            var temp = DateTime.Now;
        }

        private AdaaMobile.Controls.ExtendedGrid _extendedGrid
        {
            get { return Element as AdaaMobile.Controls.ExtendedGrid; }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<ExtendedGrid> e)
        {
            base.OnElementChanged(e);
			if (NativeView == null || _extendedGrid == null) return;
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
            var scaleFactor = UIScreen.MainScreen.Scale;
            //NativeView.Layer.BackgroundColor = Color.Transparent.ToCGColor();
            NativeView.Layer.CornerRadius = (float)_extendedGrid.CornerRadius * scaleFactor;
            NativeView.Layer.MasksToBounds = false;
            NativeView.ClipsToBounds = true;

        }


    }
}
