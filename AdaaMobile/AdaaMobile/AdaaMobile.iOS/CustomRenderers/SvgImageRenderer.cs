//Link https://github.com/BradChase2011/Xamarin.Forms.Plugins

using System;
using System.IO;
using AdaaMobile.Controls;
using AdaaMobile.iOS.CustomRenderers;
using Foundation;
using NGraphics;
using NGraphics.Parsers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(SvgImage), typeof(SvgImageRenderer))]
namespace AdaaMobile.iOS.CustomRenderers
{
    /// <summary>
    /// SVG Renderer
    /// </summary>
    [Preserve(AllMembers = true)]
    public class SvgImageRenderer : ImageRenderer, ISvgImageRenderer
    {
        /// <summary>
        ///   Used for registration with dependency service
        /// </summary>
        public static void Init()
        {
            var temp = DateTime.Now;
        }

        private SvgImage _formsControl
        {
            get { return Element as SvgImage; }
        }


        public async void Render()
        {
            if (_formsControl != null)
            {
                var svgStream = _formsControl.SvgAssembly.GetManifestResourceStream(_formsControl.SvgPath);

                if (svgStream == null)
                {
                    throw new Exception(string.Format("Error retrieving {0} make sure Build Action is Embedded Resource",
                      _formsControl.SvgPath));
                }

                var r = new SvgReader(new StreamReader(svgStream), new StylesParser(new ValuesParser()), new ValuesParser());
                this.ReplaceColors(r.Graphic, _formsControl.ReplacementColors);

                var graphics = r.Graphic;

                var width = _formsControl.Width <= 0 ? 100 : _formsControl.Width;
                var height = _formsControl.Height <= 0 ? 100 : _formsControl.Height;

                var scale = 1.0;

                if (height >= width)
                {
                    scale = height / graphics.Size.Height;
                }
                else
                {
                    scale = width / graphics.Size.Width;
                }

                var scaleFactor = UIScreen.MainScreen.Scale;

                var canvas = new ApplePlatform().CreateImageCanvas(graphics.Size, scale * scaleFactor);
                graphics.Draw(canvas);
                var image = canvas.GetImage();

                var uiImage = image.GetUIImage();
                Control.Image = uiImage;
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);
            Render();
        }


    }
}