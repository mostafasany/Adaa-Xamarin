using System;
using System.Windows.Media;
using AdaaMobile.Controls;
using AdaaMobile.WinPhone.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;

[assembly: ExportRenderer(typeof(CircleImage), typeof(ImageCircleRenderer))]
namespace AdaaMobile.WinPhone.CustomRenderers
{
    //Author: https://github.com/jamesmontemagno/Xamarin.Plugins/tree/master/ImageCircle

    /// <summary>
    /// ImageCircle Implementation
    /// </summary>
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
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      base.OnElementPropertyChanged(sender, e);
      if (Control != null && Control.Clip == null)
      {
        var min = Math.Min(Element.Width, Element.Height) / 2.0f;
        if (min <= 0)
          return;
        Control.Clip = new EllipseGeometry
        {
          Center = new System.Windows.Point(min, min),
          RadiusX = min,
          RadiusY = min
        };
      }
    }
  }
}
