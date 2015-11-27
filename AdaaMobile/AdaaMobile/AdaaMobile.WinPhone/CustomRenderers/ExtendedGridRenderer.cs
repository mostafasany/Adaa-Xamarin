using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AdaaMobile.Controls;
using AdaaMobile.CustomRenderers;
using AdaaMobile.WinPhone.CustomRenderers;
using AdaaMobile.WinPhone.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;
using Rectangle = System.Windows.Shapes.Rectangle;

[assembly: ExportRenderer(typeof(ExtendedGrid), typeof(ExtendedGridRenderer))]

namespace AdaaMobile.WinPhone.CustomRenderers
{
    //TODO:Test it
    public class ExtendedGridRenderer : ViewRenderer
    {
        private AdaaMobile.Controls.ExtendedGrid _formControl
        {
            get { return (ExtendedGrid)Element; }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);
            if (Control == null) return;
            Control.SizeChanged -= Control_SizeChanged;
            Control.SizeChanged += Control_SizeChanged;
        }

        private void Control_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateCornerRadius();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == AdaaMobile.Controls.ExtendedGrid.CornerRadiusProperty.PropertyName)
            {
                UpdateCornerRadius();
            }
        }
        private void UpdateCornerRadius()
        {
            if (Control != null && Control.Clip == null)
            {
                var relativeRadius = _formControl.CornerRadius * 1.25;

                Control.Clip = new RectangleGeometry()
                {
                    Rect = new Rect(0, 0, Width, Height),
                    RadiusX = relativeRadius,
                    RadiusY = relativeRadius
                };
            }
        }
    }
}
