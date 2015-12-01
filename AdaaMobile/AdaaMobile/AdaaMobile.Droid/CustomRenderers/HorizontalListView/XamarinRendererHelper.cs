using System;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace AdaaMobile.Droid.CustomRenderers.HorizontalListView
{
    public class XamarinRendererHelper
    {
        // Solution for render issue added from these links
        // Thanks for thaihung203 for the brilliant solution
        // See https://forums.xamarin.com/discussion/comment/148210/#Comment_148210
        // and https://github.com/thaihung203/xfpopup/blob/master/xfpopup/xfpopup.Droid/DroidXFPopupSrvc.cs
        private static Type _platformType = Type.GetType("Xamarin.Forms.Platform.Android.Platform, Xamarin.Forms.Platform.Android", true);

        private static BindableProperty _rendererProperty;

        public static BindableProperty RendererProperty
        {
            get { return _rendererProperty ?? (_rendererProperty = (BindableProperty)_platformType.GetField("RendererProperty", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public).GetValue(null)); }
        }

        private static PropertyInfo _isplatformenabledprop;

        public static PropertyInfo IsPlatformEnabledProperty
        {
            get { return _isplatformenabledprop ?? (_isplatformenabledprop = typeof(VisualElement).GetProperty("IsPlatformEnabled", BindingFlags.NonPublic | BindingFlags.Instance)); }
        }

        private static PropertyInfo _platform;

        public static PropertyInfo PlatformProperty
        {
            get { return _platform ?? (_platform = typeof(VisualElement).GetProperty("Platform", BindingFlags.NonPublic | BindingFlags.Instance)); }
        }

        public static IVisualElementRenderer Convert(Xamarin.Forms.View source, Xamarin.Forms.View valid)
        {
            IVisualElementRenderer render = (IVisualElementRenderer)source.GetValue(RendererProperty);
            if (render == null)
            {
                render = Platform.CreateRenderer(source);
                source.SetValue(RendererProperty, render);
                var p = PlatformProperty.GetValue(valid);
                PlatformProperty.SetValue(source, p);
                IsPlatformEnabledProperty.SetValue(source, true);
            }

            return render;
        }
    }
}