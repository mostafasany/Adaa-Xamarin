﻿using System;
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
using TextAlignment = Android.Views.TextAlignment;

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

            //if (Control != null && e.NewElement != null && e.NewElement.IsPassword)
            //{
            //    Control.SetTypeface(Typeface.Default, TypefaceStyle.Normal);
            //    //Control.TransformationMethod = new PasswordTransformationMethod();
            //}

            SetFont(view);
            SetTextAlignment(view);
            SetHasBorder(view);
            SetPlaceholderTextColor(view);
            SetMaxLength(view);

        }


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
                SetHasBorder(view);
            }
            else if (e.PropertyName == ExtendedEntry.PlaceholderTextColorProperty.PropertyName)
            {
                SetPlaceholderTextColor(view);
            }
            else
            {
                base.OnElementPropertyChanged(sender, e);
                if (e.PropertyName == ExtendedEntry.IsPasswordProperty.PropertyName)
                {
                    //to fix issue in password hint
                    if(Element.IsPassword)
                    Control.SetTypeface(Typeface.Default, TypefaceStyle.Normal);
                }
                else
                if (e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName)
                {
                    this.Control.SetBackgroundColor(view.BackgroundColor.ToAndroid());
                }
            }
        }

        private void IsPasswordFix(ExtendedEntry entry)
        {
            if (entry.IsPassword)
            {
                Control.InputType = (InputTypes.ClassText | InputTypes.TextVariationPassword);
            }
            else
            {

                Control.InputType = (InputTypes.ClassText);
            }
        }

        private void SetHasBorder(ExtendedEntry entry)
        {
            if (!entry.HasBorder)
            {
                Control.Background.SetColorFilter(entry.BackgroundColor.ToAndroid(), PorterDuff.Mode.SrcIn);
                //Control.Background.ClearColorFilter();
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
                    var androidLocale = Java.Util.Locale.Default;
                    if (androidLocale.Language.StartsWith("en"))
                    {
                        Control.Gravity = GravityFlags.Start;
                    }
                    else
                    {
                        Control.Gravity = GravityFlags.Right;
                    }
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

