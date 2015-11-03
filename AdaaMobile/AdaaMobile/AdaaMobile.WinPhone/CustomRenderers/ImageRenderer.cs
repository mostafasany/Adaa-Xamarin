using System.IO;
using AdaaMobile.WinPhone.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;

[assembly: ExportRenderer(typeof(Image), typeof(LocalizedImageRenderer))]

namespace AdaaMobile.WinPhone.CustomRenderers
{
    public class LocalizedImageRenderer : ImageRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var s = e.NewElement.Source as FileImageSource;
                if (s != null)
                {
                    var fileName = s.File;
                    string ci = System.Threading.Thread.CurrentThread.CurrentUICulture.ToString();
                    // you might need some custom logic here to support particular cultures and fallbacks
                    if (ci == "pt-BR")
                    {
                        // use the complete string 'as is'
                    }
                    else if (ci == "zh-CN")
                    {
                        // we could have named the image directories differently,
                        // but this keeps them consisent with RESX file naming
                        ci = "zh-Hans";
                    }
                    else if (ci == "zh-TW" || ci == "zh-HK")
                    {
                        ci = "zh-Hant";
                    }
                    else
                    {
                        // for all others, just use the two-character language code
                        ci = System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
                    }
                    e.NewElement.Source = Path.Combine("Assets/" + ci + "/" + fileName);
                }
            }
        }
    }
}