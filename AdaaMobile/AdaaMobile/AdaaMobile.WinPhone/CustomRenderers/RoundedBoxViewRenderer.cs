//Author https://github.com/paulpatarinski/Xamarin.Forms.Plugins/tree/master/RoundedBoxView

using System.ComponentModel;
using System.Windows.Controls;
using AdaaMobile.WinPhone.CustomRenderers;
using AdaaMobile.WinPhone.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;

[assembly:
  ExportRenderer(typeof (AdaaMobile.Controls.RoundedBoxView), typeof (RoundedBoxViewRenderer))]

namespace AdaaMobile.WinPhone.CustomRenderers
{
  /// <summary>
  ///   RoundedBoxView Renderer
  /// </summary>
  public class RoundedBoxViewRenderer : ViewRenderer<AdaaMobile.Controls.RoundedBoxView, Border>
  {
    /// <summary>
    ///   Used for registration with dependency service
    /// </summary>
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

      var border = new Border();

      border.InitializeFrom(_formControl);

      SetNativeControl(border);
    }

    protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      base.OnElementPropertyChanged(sender, e);

      Control.UpdateFrom(_formControl, e.PropertyName);
    }
  }
}