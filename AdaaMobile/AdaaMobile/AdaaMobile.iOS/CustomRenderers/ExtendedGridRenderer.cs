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
        public  static void Init()
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
			if (NativeView == null ) return;
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
            NativeView.Layer.BackgroundColor = Color.Transparent.ToCGColor();
			NativeView.Layer.CornerRadius = (float)_extendedGrid.CornerRadius * scaleFactor;
			NativeView.Layer.MasksToBounds = true;
			NativeView.ClipsToBounds = true;
			ClipsToBounds = true;

//			float fCornerRadius = 25f;
//
//			// Add a layer that holds the rounded corners.
//			UIBezierPath oMaskPath =  UIBezierPath.FromRoundedRect (NativeView.Bounds, UIRectCorner.AllCorners, new CGSize (fCornerRadius, fCornerRadius));
//
//			CAShapeLayer oMaskLayer = new CAShapeLayer ();
//			oMaskLayer.Frame = NativeView.Bounds;
//			oMaskLayer.Path = oMaskPath.CGPath;
//
//			// Set the newly created shape layer as the mask for the image view's layer
//			NativeView.Layer.Mask = oMaskLayer;
        }


    }
}
