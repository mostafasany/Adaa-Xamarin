using System;
using System.ComponentModel;
using System.Diagnostics;
using AdaaMobile.Controls;
using AdaaMobile.iOS.CustomRenderers;
using Foundation;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
#if __UNIFIED__

#else
using MonoTouch.Foundation;
#endif
//Author: https://github.com/jamesmontemagno/Xamarin.Plugins/tree/master/ImageCircle

[assembly: ExportRenderer(typeof(CircleImage), typeof(ImageCircleRenderer))]
namespace AdaaMobile.iOS.CustomRenderers
{
  /// <summary>
  /// ImageCircle Implementation
  /// </summary>
  [Preserve(AllMembers=true)]
  public class ImageCircleRenderer : ImageRenderer
  {
    /// <summary>
    /// Used for registration with dependency service
    /// </summary>
		public async static void Init()
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
      if (Element == null)
        return;
      CreateCircle();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      base.OnElementPropertyChanged(sender, e);
      if (e.PropertyName == VisualElement.HeightProperty.PropertyName ||
          e.PropertyName == VisualElement.WidthProperty.PropertyName ||
        e.PropertyName == CircleImage.BorderColorProperty.PropertyName ||
        e.PropertyName == CircleImage.BorderThicknessProperty.PropertyName)
      {
        CreateCircle();
      }
    }

    private void CreateCircle()
    {
      try
      {
        double min = Math.Min(Element.Width, Element.Height);
        Control.Layer.CornerRadius = (float)(min / 2.0);
        Control.Layer.MasksToBounds = false;
        Control.Layer.BorderColor = ((CircleImage)Element).BorderColor.ToCGColor();
        Control.Layer.BorderWidth = ((CircleImage)Element).BorderThickness;
        Control.ClipsToBounds = true;
      }
      catch(Exception ex)
      {
        Debug.WriteLine("Unable to create circle image: " + ex);
      }
    }
  }
}
