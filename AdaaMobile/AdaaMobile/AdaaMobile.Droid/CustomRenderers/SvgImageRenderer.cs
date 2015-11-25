//Link https://github.com/BradChase2011/Xamarin.Forms.Plugins

using System;
using System.IO;
using System.Threading.Tasks;
using AdaaMobile.Controls;
using AdaaMobile.Droid.CustomRenderers;
using Android.Runtime;
using Android.Widget;
using NGraphics;
using NGraphics.Parsers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(SvgImage), typeof(SvgImageRenderer))]
namespace AdaaMobile.Droid.CustomRenderers
{
    [Preserve(AllMembers = true)]
    public class SvgImageRenderer : ViewRenderer<SvgImage, ImageView>, ISvgImageRenderer
    {
        public static void Init()
        {
            var temp = DateTime.Now;
        }

        private SvgImage _formsControl
        {
            get
            {
                return Element as SvgImage;
            }
        }

        public async void Render()
        {
            //added check because both *may* not be set at the same time, this negates the error throwing unfortuantely. --bwc
            if (String.IsNullOrEmpty(Element.SvgPath) || (Element.SvgAssembly == null))
                return;

            if (_formsControl != null)
            {
                await Task.Run(() =>
               {
                   Stream svgStream = _formsControl.SvgAssembly.GetManifestResourceStream(_formsControl.SvgPath);

                   if (svgStream == null)
                       throw new Exception(string.Format("Error retrieving {0} make sure Build Action is Embedded Resource", _formsControl.SvgPath));

                   SvgReader r = new SvgReader(new StreamReader(svgStream), new StylesParser(new ValuesParser()), new ValuesParser());

                   this.ReplaceColors(r.Graphic, Element.ReplacementColors);

                   Graphic graphics = r.Graphic;
                   int width = PixelToDP((int)_formsControl.WidthRequest <= 0 ? 100 : (int)_formsControl.WidthRequest);
                   int height = PixelToDP((int)_formsControl.HeightRequest <= 0 ? 100 : (int)_formsControl.HeightRequest);
                   double scale = 1.0;

                   if (height >= width)
                       scale = height / graphics.Size.Height;
                   else
                       scale = width / graphics.Size.Width;

                   IImageCanvas canvas = new AndroidPlatform().CreateImageCanvas(graphics.Size, scale);

                   graphics.Draw(canvas);
                   BitmapImage image = (BitmapImage)canvas.GetImage();


                   return image;
               }).ContinueWith(taskResult =>
               {
                   Device.BeginInvokeOnMainThread(() =>
                   {
                       var imageView = new ImageView(Context);
                       imageView.SetScaleType(ImageView.ScaleType.FitXy);
                       imageView.SetImageBitmap(taskResult.Result.Bitmap);
                       SetNativeControl(imageView);
                   });
               });
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<SvgImage> e)
        {
            base.OnElementChanged(e);
            Render();
        }

        public override SizeRequest GetDesiredSize(int widthConstraint, int heightConstraint)
        {
            return new SizeRequest(new Size(_formsControl.WidthRequest, _formsControl.WidthRequest));
        }

        /// <summary>
        /// http://stackoverflow.com/questions/24465513/how-to-get-detect-screen-size-in-xamarin-forms
        /// </summary>
        /// <param name="pixel"></param>
        /// <returns></returns>
        private int PixelToDP(int pixel)
        {
            var scale = Resources.DisplayMetrics.Density;
            return (int)((pixel * scale) + 0.5f);
        }
    }
}