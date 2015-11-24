//Author https://github.com/paulpatarinski/Xamarin.Forms.Plugins/tree/master/RoundedBoxView

using System;
using System.ComponentModel;
using AdaaMobile.Controls;
using AdaaMobile.Droid.CustomRenderers;
using AdaaMobile.Droid.Extensions;
using Android.Graphics;
using Android.Runtime;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using View = Android.Views.View;

[assembly:
  ExportRenderer(typeof(AdaaMobile.Controls.RoundedBoxView), typeof(RoundedBoxViewRenderer))]

namespace AdaaMobile.Droid.CustomRenderers
{
    [Preserve(AllMembers = true)]
    public class RoundedBoxViewRenderer : ViewRenderer<AdaaMobile.Controls.RoundedBoxView, View>
    {
        public static void Init()
        {
        }

        private AdaaMobile.Controls.RoundedBoxView _formControl
        {
            get { return Element; }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<AdaaMobile.Controls.RoundedBoxView> e)
        {
            base.OnElementChanged(e);

            this.InitializeFrom(_formControl);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == RoundedBoxView.CornerRadiusProperty.PropertyName)
            {
                Invalidate();
            }
            this.UpdateFrom(_formControl, e.PropertyName);
            
        }

        protected override bool DrawChild(Canvas canvas, View child, long drawingTime)
        {
            //var clippingPath = new Path();
            //RectF rect = new RectF(0, 0, Width, Height);
            //clippingPath.AddRoundRect(rect, (float)_formControl.CornerRadius, (float)_formControl.CornerRadius, Path.Direction.Cw);
            //canvas.Save();
            //canvas.ClipPath(clippingPath);
            //var result= base.DrawChild(canvas, child, drawingTime);
            
            //canvas.Restore();
            //return result;

            var radius = Math.Min(Width, Height) / 2;
            var strokeWidth = (_formControl).BorderThickness;
            radius -= strokeWidth / 2;


            Path path = new Path();
            path.AddCircle(Width / 2, Height / 2, radius, Path.Direction.Ccw);
            canvas.Save();
            canvas.ClipPath(path);

            var result = base.DrawChild(canvas, child, drawingTime);

            canvas.Restore();
            return result;
        }
    }
}