using System.ComponentModel;
using System.Threading.Tasks;
using AdaaMobile.Controls;
using AdaaMobile.CustomRenderers;
using AdaaMobile.Droid.CustomRenderers;
using AdaaMobile.Droid.Extensions;
using AdaaMobile.Enums;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

//Author https://github.com/XLabs/Xamarin-Forms-Labs/blob/master/src/Forms/XLabs.Forms/Controls/
[assembly: ExportRenderer(typeof(ImageButton), typeof(ImageButtonRenderer))]
// ReSharper disable once CheckNamespace
namespace AdaaMobile.CustomRenderers
{
    /// <summary>
    /// Draws a button on the Android platform with the image shown in the right 
    /// position with the right size.
    /// </summary>
    public partial class ImageButtonRenderer : ButtonRenderer
    {
        /// <summary>
        /// Gets the underlying control typed as an <see cref="ImageButton"/>.
        /// </summary>
        private Controls.ImageButton ImageButton
        {
            get { return (Controls.ImageButton)Element; }
        }

        /// <summary>
        /// Sets up the button including the image. 
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected async override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            var targetButton = this.Control;

            if (targetButton == null || e.NewElement == null)
                return;

            if (this.Element.Font != Font.Default)
            {
                targetButton.Typeface = e.NewElement.Font.ToExtendedTypeface(Context);
            }

            if (this.ImageButton.Source != null || this.ImageButton.DisabledSource != null)
            {
                await this.SetImageSourceAsync(targetButton, e.NewElement as Controls.ImageButton);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing && this.Control != null)
            {
                this.Control.Dispose();
            }
        }


        /// <summary>
        /// Sets the image source.
        /// </summary>
        /// <param name="targetButton">The target button.</param>
        /// <param name="model">The model.</param>
        /// <returns>A <see cref="Task"/> for the awaited operation.</returns>
        private async Task SetImageSourceAsync(Android.Widget.Button targetButton, Controls.ImageButton model)
        {
            if (targetButton == null || model == null)
                return;

            const int Padding = 10;
            var source = model.IsEnabled ? model.Source : model.DisabledSource ?? model.Source;

            using (var bitmap = await this.GetBitmapAsync(source))
            {
                if (bitmap != null)
                {
                    var drawable = new BitmapDrawable(bitmap);
                    var tintColor = model.IsEnabled ? model.ImageTintColor : model.DisabledImageTintColor;
                    if (tintColor != Xamarin.Forms.Color.Transparent)
                    {
                        drawable.SetTint(tintColor.ToAndroid());
                        drawable.SetTintMode(PorterDuff.Mode.SrcIn);
                    }

                    using (var scaledDrawable = GetScaleDrawable(drawable, RequestToPixels(GetWidth(model.ImageWidthRequest)),
                                                   RequestToPixels(GetHeight(model.ImageHeightRequest))))
                    {
                        Drawable left = null;
                        Drawable right = null;
                        Drawable top = null;
                        Drawable bottom = null;
                        targetButton.CompoundDrawablePadding = Padding;

//                        int controlPadding = 1;
//                        targetButton.SetPadding(controlPadding, controlPadding, controlPadding, controlPadding);

                        switch (model.Orientation)
                        {
                            //Should be left, and Right, modify later, and extend with new orientation options
                            case ImageOrientation.ImageToLeft:
                                targetButton.Gravity = GravityFlags.CenterHorizontal | GravityFlags.CenterVertical;
                                left = scaledDrawable;
                                break;
                            case ImageOrientation.ImageToRight:
                                targetButton.Gravity = GravityFlags.CenterHorizontal | GravityFlags.CenterVertical;
                                right = scaledDrawable;
                                break;
                            case ImageOrientation.ImageOnTop:
                                top = scaledDrawable;
                                break;
                            case ImageOrientation.ImageOnBottom:
                                bottom = scaledDrawable;
                                break;
                        }

                        targetButton.SetCompoundDrawables(left, top, right, bottom);
                    }
                }
            }
        }

        /// <summary>
        /// Gets a <see cref="Bitmap"/> for the supplied <see cref="ImageSource"/>.
        /// </summary>
        /// <param name="source">The <see cref="ImageSource"/> to get the image for.</param>
        /// <returns>A loaded <see cref="Bitmap"/>.</returns>
        private async Task<Bitmap> GetBitmapAsync(ImageSource source)
        {
            var handler = AdaaMobile.CustomRenderers.ImageButtonRenderer.GetHandler(source);
            var returnValue = (Bitmap)null;

            if (handler != null)
                returnValue = await handler.LoadImageAsync(source, this.Context);

            return returnValue;
        }

        /// <summary>
        /// Called when the underlying model's properties are changed.
        /// </summary>
        /// <param name="sender">The Model used.</param>
        /// <param name="e">The event arguments.</param>
        protected async override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == Controls.ImageButton.SourceProperty.PropertyName ||
                e.PropertyName == Controls.ImageButton.DisabledSourceProperty.PropertyName ||
                e.PropertyName == VisualElement.IsEnabledProperty.PropertyName ||
                e.PropertyName == Controls.ImageButton.ImageTintColorProperty.PropertyName ||
                e.PropertyName == Controls.ImageButton.DisabledImageTintColorProperty.PropertyName)
            {
                await SetImageSourceAsync(this.Control, this.ImageButton);
            }
        }

        /// <summary>
        /// Returns a <see cref="Drawable"/> with the correct dimensions from an 
        /// Android resource id.
        /// </summary>
        /// <param name="drawable">An android <see cref="Drawable"/>.</param>
        /// <param name="width">The width to scale to.</param>
        /// <param name="height">The height to scale to.</param>
        /// <returns>A scaled <see cref="Drawable"/>.</returns>
        private Drawable GetScaleDrawable(Drawable drawable, int width, int height)
        {
            var returnValue = new ScaleDrawable(drawable, 0, width, height).Drawable;
            returnValue.SetBounds(0, 0, width, height);
            return returnValue;
        }

        /// <summary>
        /// Returns a drawable dimension modified according to the current display DPI.
        /// </summary>
        /// <param name="sizeRequest">The requested size in relative units.</param>
        /// <returns>Size in pixels.</returns>
        public int RequestToPixels(int sizeRequest)
        {
            return (int)(sizeRequest * Resources.DisplayMetrics.Density);
        }

    }
}
