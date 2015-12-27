using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using AdaaMobile.Controls;
using AdaaMobile.Droid.CustomRenderers;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Android.Graphics.Color;
using Math = System.Math;
using View = Android.Views.View;

[assembly: ExportRenderer(typeof(ExtendedGrid), typeof(ExtendedGridRenderer))]
namespace AdaaMobile.Droid.CustomRenderers
{
    [Preserve(AllMembers = true)]
    public class ExtendedGridRenderer : ViewRenderer<ExtendedGrid, View>
    {
        private ExtendedGrid ExtendedGrid
        {
            get { return Element; }
        }
        private Canvas _temp;
        private Paint _paint;
        private Paint _p = new Paint();
        private Paint _transparentPaint;
        public ExtendedGridRenderer()
        {
            Log.Debug("ExtendedGrid", "const");

        }

        /// <summary>
        /// Used for registration with dependency service
        /// </summary>
        public static void Init()
        {
            var temp = DateTime.Now;
        }

        private void Prepare()
        {
            Bitmap bitmap = Bitmap.CreateBitmap(Width, Height, Bitmap.Config.Argb8888);

            _temp = new Canvas(bitmap);
            _paint = new Paint();
            _paint.SetARGB(125, 0, 0, 0);
            _transparentPaint = new Paint();
            _transparentPaint.Color = Color.Transparent;
            _transparentPaint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.Clear));
        }
        protected override void OnElementChanged(ElementChangedEventArgs<ExtendedGrid> e)
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
            Log.Debug("ExtendedGrid", "OnElementChanged");

            SetWillNotDraw(false);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            Log.Debug("ExtendedGrid", "OnElementPropertyChanged");

            if (e.PropertyName == ExtendedGrid.CornerRadiusProperty.PropertyName)
            {
                Invalidate();
            }
        }

        //public override void Draw(Canvas canvas)
        //{

        //    try
        //    {

        //        //canvas.DrawColor(Color.Argb(125, 0, 0, 0), PorterDuff.Mode.Clear);
        //        this.Control.ClipToOutline = false;
        //        //Control.SetBackgroundColor(ExtendedGrid.BackgroundColor.ToAndroid());
        //        var clippingPath = new Path();
        //        RectF rect = new RectF(0, 0, Width, Height);
        //        clippingPath.AddRoundRect(rect, (float)ExtendedGrid.CornerRadius * 5, (float)ExtendedGrid.CornerRadius * 5, Path.Direction.Ccw);
        //        canvas.Save();
        //        canvas.ClipPath(clippingPath);
        //        base.Draw(canvas);
        //        canvas.Restore();
        //    }
        //    catch (System.Exception)
        //    {
        //        //ignored
        //    }

        //    //In case it's failed
        //    //base.Draw(canvas);
        //}

        protected override bool DrawChild(Canvas canvas, View child, long drawingTime)
        {
            var clippingPath = new Path();
            RectF rect = new RectF(0, 0, Width, Height);
            double cornderRadius = PixelToDp(ExtendedGrid.CornerRadius);
            clippingPath.AddRoundRect(rect, (float)cornderRadius, (float)cornderRadius, Path.Direction.Ccw);
            canvas.Save();
            canvas.ClipPath(clippingPath);
            var result = base.DrawChild(canvas, child, drawingTime);
            canvas.Restore();
            return result;
        }

        /// <summary>
        /// http://stackoverflow.com/questions/24465513/how-to-get-detect-screen-size-in-xamarin-forms
        /// </summary>
        /// <param name="pixel"></param>
        /// <returns></returns>
        private double PixelToDp(double pixel)
        {
            var scale = Resources.DisplayMetrics.Density;
            return (double)((pixel * scale) + 0.5f);
        }
    }
}