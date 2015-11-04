using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Refractored.Xam.Settings;
using Xamarin.Forms;

namespace AdaaMobile.Helpers
{
    public class AppSettings : IAppSettings
    {

        private const string SelectedCultureKey = "SelectedCultureKey";
        private readonly string _selectedCultureDefault = "en-US";

        public string SelectedCultureName
        {
            get
            {
                return CrossSettings.Current.GetValueOrDefault(SelectedCultureKey, _selectedCultureDefault);
            }
            set { CrossSettings.Current.AddOrUpdateValue(SelectedCultureKey, value); }
        }
    }
}
