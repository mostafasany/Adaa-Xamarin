using System;
using AdaaMobile.Droid.CustomRenderers;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ScrollView), typeof(InnerAndroidScrollViewRenderer))]

//Original link:https://forums.xamarin.com/discussion/20834/horizontal-scrollview-within-vertical-scrollview
namespace AdaaMobile.Droid.CustomRenderers
{
    internal class InnerAndroidScrollViewRenderer : ScrollViewRenderer
    {
        private float _startX;
        private float _startY;
        private int _isHorizontal = -1;

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            if (((ScrollView)e.NewElement).Orientation == ScrollOrientation.Horizontal) _isHorizontal = 1;

        }

        public override bool DispatchTouchEvent(MotionEvent e)
        {

            switch (e.Action)
            {
                case MotionEventActions.Down:
                    _startX = e.RawX;
                    _startY = e.RawY;
                    Parent.RequestDisallowInterceptTouchEvent(true);
                    break;
                case MotionEventActions.Move:
                    if (_isHorizontal * Math.Abs(_startX - e.RawX) < _isHorizontal * Math.Abs(_startY - e.RawY))
                        Parent.RequestDisallowInterceptTouchEvent(false);
                    break;
                case MotionEventActions.Up:
                    Parent.RequestDisallowInterceptTouchEvent(false);
                    break;
            }

            return base.DispatchTouchEvent(e);
        }
    }
}