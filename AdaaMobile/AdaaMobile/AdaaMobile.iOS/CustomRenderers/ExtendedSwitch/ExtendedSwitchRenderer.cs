using System;
using AdaaMobile.Controls;
using AdaaMobile.iOS.CustomRenderers.ExtendedSwitch;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

//Control Original Link https://github.com/XLabs/Xamarin-Forms-Labs/blob/master/src/Forms/
[assembly: ExportRenderer(typeof(ExtendedSwitch), typeof(ExtendedSwitchRenderer))]

namespace AdaaMobile.iOS.CustomRenderers.ExtendedSwitch
{
    /// <summary>
	/// Class ExtendedSwitchRenderer.
	/// </summary>
	public class ExtendedSwitchRenderer : ViewRenderer<Controls.ExtendedSwitch, UISwitch>
    {
        /// <summary>
        /// Called when [element changed].
        /// </summary>
        /// <param name="e">The e.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<Controls.ExtendedSwitch> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                e.OldElement.Toggled -= ElementToggled;
            }

            if (e.NewElement != null)
            {
                SetNativeControl(new UISwitch());
                Control.On = e.NewElement.IsToggled;
                Control.ValueChanged += ControlValueChanged;
                SetTintColor(Element.TintColor.ToUIColor());
                Element.Toggled += ElementToggled;
            }
        }

        /// <summary>
        /// Handles the <see cref="E:ElementPropertyChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == "TintColor")
            {
                SetTintColor(Element.TintColor.ToUIColor());
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Control.ValueChanged -= ControlValueChanged;
                Element.Toggled -= ElementToggled;
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Sets the color of the tint.
        /// </summary>
        /// <param name="color">The color.</param>
        private void SetTintColor(UIColor color)
        {
            Control.TintColor = color;
            //this.Control.ThumbTintColor = color;
            Control.OnTintColor = color;
        }

        /// <summary>
        /// Elements the toggled.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ToggledEventArgs"/> instance containing the event data.</param>
        private void ElementToggled(object sender, ToggledEventArgs e)
        {
            Control.SetState(Element.IsToggled, true);
        }

        /// <summary>
        /// Controls the value changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ControlValueChanged(object sender, EventArgs e)
        {
            Element.IsToggled = Control.On;
        }
    }
}