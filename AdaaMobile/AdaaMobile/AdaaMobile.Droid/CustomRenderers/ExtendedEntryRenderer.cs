using System.ComponentModel;
using AdaaMobile.Controls;
using AdaaMobile.Droid.CustomRenderers;
using AdaaMobile.Droid.Extensions;
using Android.Graphics;
using Android.Text;
using Android.Text.Method;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExtendedEntry), typeof(ExtendedEntryRenderer))]
namespace AdaaMobile.Droid.CustomRenderers
{
    using Color = Xamarin.Forms.Color;

    /// <summary>
    /// Class ExtendedEntryRenderer.
    /// </summary>
    public class ExtendedEntryRenderer : EntryRenderer
    {
        private const int MinDistance = 10;

        private float downX, downY, upX, upY;

        /// <summary>
        /// Called when [element changed].
        /// </summary>
        /// <param name="e">The e.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            var view = (ExtendedEntry)Element;
            
            if (Control != null && e.NewElement != null && e.NewElement.IsPassword)
            {
                Control.SetTypeface(Typeface.Default, TypefaceStyle.Normal);
                Control.TransformationMethod = new PasswordTransformationMethod();
            }

            SetFont(view);
            SetTextAlignment(view);
            //SetBorder(view);
            SetPlaceholderTextColor(view);
            SetMaxLength(view);

        }

        /// <summary>
   
        /// <summary>
        /// Handles the <see cref="E:ElementPropertyChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var view = (ExtendedEntry)Element;

            if (e.PropertyName == ExtendedEntry.FontProperty.PropertyName)
            {
                SetFont(view);
            }
            else if (e.PropertyName == ExtendedEntry.XAlignProperty.PropertyName)
            {
                SetTextAlignment(view);
            }
            else if (e.PropertyName == ExtendedEntry.HasBorderProperty.PropertyName)
            {
                //return;   
            }
            else if (e.PropertyName == ExtendedEntry.PlaceholderTextColorProperty.PropertyName)
            {
                SetPlaceholderTextColor(view);
            }
            else
            {
                base.OnElementPropertyChanged(sender, e);
                if (e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName)
                {
                    this.Control.SetBackgroundColor(view.BackgroundColor.ToAndroid());
                }
            }
        }

        /// <summary>
        /// Sets the text alignment.
        /// </summary>
        /// <param name="view">The view.</param>
        private void SetTextAlignment(ExtendedEntry view)
        {
            switch (view.XAlign)
            {
                case Xamarin.Forms.TextAlignment.Center:
                    Control.Gravity = GravityFlags.CenterHorizontal;
                    break;
                case Xamarin.Forms.TextAlignment.End:
                    Control.Gravity = GravityFlags.End;
                    break;
                case Xamarin.Forms.TextAlignment.Start:
                    Control.Gravity = GravityFlags.Start;
                    break;
            }
        }

        /// <summary>
        /// Sets the font.
        /// </summary>
        /// <param name="view">The view.</param>
        private void SetFont(ExtendedEntry view)
        {
            if (view.Font != Font.Default) 
            {
                Control.TextSize = view.Font.ToScaledPixel();
                Control.Typeface = view.Font.ToExtendedTypeface(Context);
            }
        }

        /// <summary>
        /// Sets the color of the placeholder text.
        /// </summary>
        /// <param name="view">The view.</param>
        private void SetPlaceholderTextColor(ExtendedEntry view)
        {
            if (view.PlaceholderTextColor != Color.Default)
            {
                Control.SetHintTextColor(view.PlaceholderTextColor.ToAndroid());	
            }		
        }

        /// <summary>
        /// Sets the MaxLength characteres.
        /// </summary>
        /// <param name="view">The view.</param>
        private void SetMaxLength(ExtendedEntry view)
        {
            Control.SetFilters(new IInputFilter[] { new InputFilterLengthFilter(view.MaxLength) });
        }
    }
}

