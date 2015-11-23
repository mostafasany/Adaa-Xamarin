using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.Helpers;
using AdaaMobile.WinPhone.Helpers;
using Xamarin.Forms;

[assembly: Dependency(typeof(Localize))]

namespace AdaaMobile.WinPhone.Helpers
{
    public class Localize : ILocalize
    {
        public System.Globalization.CultureInfo GetCurrentCultureInfo()
        {
            return System.Threading.Thread.CurrentThread.CurrentUICulture;
        }

        public void UpdateCultureInfo(string cultureName)
        {
            throw new System.NotImplementedException();
        }
    }

}
