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

		private const string UserTokenKey = "UserTokenKey";
		private readonly string _userTokenDefault = string.Empty;


        public string SelectedCultureName
        {
            get
            {
                return CrossSettings.Current.GetValueOrDefault(SelectedCultureKey, _selectedCultureDefault);
            }
            set { CrossSettings.Current.AddOrUpdateValue(SelectedCultureKey, value); }
        }

		public string UserToken
		{
			get
			{
				return CrossSettings.Current.GetValueOrDefault(UserTokenKey, _userTokenDefault);
			}
			set { CrossSettings.Current.AddOrUpdateValue(UserTokenKey, value); }
		}
    }
}
