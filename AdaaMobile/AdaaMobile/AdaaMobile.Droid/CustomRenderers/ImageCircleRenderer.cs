using System;
using System.ComponentModel;
using AdaaMobile.Controls;
using AdaaMobile.Droid.CustomRenderers;
using Android.Graphics;
using Android.Runtime;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
//Author: https://github.com/jamesmontemagno/Xamarin.Plugins/tree/master/ImageCircle

[assembly: ExportRenderer(typeof(CircleImage), typeof(ImageCircleRenderer))]
namespace AdaaMobile.Droid.CustomRenderers
{
    /// <summary>
    /// ImageCircle Implementation
    /// </summary>
    [Preserve(AllMembers = true)]
    public class ImageCircleRenderer : ImageRenderer
    {
        /// <summary>
        /// Used for registration with dependency service
        /// </summary>
        public  static void Init()
        {
            var temp = DateTime.Now;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                //Only enable hardware accelleration on lollipop
                if ((int)Android.OS.Build.VERSION.SdkInt < 21)
                {
                    SetLayerType(LayerType.Software, null);
                }

            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == CircleImage.BorderColorProperty.PropertyName ||
              e.PropertyName == CircleImage.BorderThicknessProperty.PropertyName)
            {
                this.Invalidate();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="child"></param>
        /// <param name="drawingTime"></param>
        /// <returns></returns>
        protected override bool DrawChild(Canvas canvas, global::Android.Views.View child, long drawingTime)
        {
            try
            {

                var radius = Math.Min(Width, Height) / 2;
                var strokeWidth = ((CircleImage)Element).BorderThickness;
                radius -= strokeWidth / 2;


                Path path = new Path();
                path.AddCircle(Width / 2, Height / 2, radius, Path.Direction.Ccw);
                canvas.Save();
                canvas.ClipPath(path);

                var result = base.DrawChild(canvas, child, drawingTime);

                canvas.Restore();

                path = new Path();
                path.AddCircle(Width / 2, Height / 2, radius, Path.Direction.Ccw);

                var thickness = ((CircleImage)Element).BorderThickness;
                if(thickness > 0.0f)
                {
                    var paint = new Paint();
                    paint.AntiAlias = true;
                    paint.StrokeWidth = thickness;
                    paint.SetStyle(Paint.Style.Stroke);
                    paint.Color = ((CircleImage)Element).BorderColor.ToAndroid();
                    canvas.DrawPath(path, paint);
                    paint.Dispose();
                }

                path.Dispose();
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Unable to create circle image: " + ex);
            }

            return base.DrawChild(canvas, child, drawingTime);
        }
    }
}
