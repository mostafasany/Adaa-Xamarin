using System;
using System.ComponentModel;
using AdaaMobile.iOS.CustomRenderers;
using AdaaMobile.iOS.Extensions;
using Foundation;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly:
  ExportRenderer(typeof (AdaaMobile.Controls.RoundedBoxView), typeof (RoundedBoxViewRenderer))]
//Author https://github.com/paulpatarinski/Xamarin.Forms.Plugins/tree/master/RoundedBoxView
namespace AdaaMobile.iOS.CustomRenderers
{
  /// <summary>
  ///   Source From : https://gist.github.com/rudyryk/8cbe067a1363b45351f6
  /// </summary>
  [Preserve(AllMembers = true)]
  public class RoundedBoxViewRenderer : BoxRenderer
  {
    /// <summary>
    ///   Used for registration with dependency service
    /// </summary>
    public static void Init()
    {
		var temp = DateTime.Now;
    }

    private AdaaMobile.Controls.RoundedBoxView _formControl
    {
      get { return Element as AdaaMobile.Controls.RoundedBoxView; }
    }

    protected override void OnElementChanged(ElementChangedEventArgs<BoxView> e)
    {
      base.OnElementChanged(e);

      this.InitializeFrom(_formControl);
    }

    protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      base.OnElementPropertyChanged(sender, e);

      this.UpdateFrom(_formControl, e.PropertyName);
    }

  }
}
