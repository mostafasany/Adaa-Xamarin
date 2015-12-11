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

		private const string NextSelectedCultureKey = "NextSelectedCultureKey";
		private readonly string _nextselectedCultureDefault = string.Empty;

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

		public string NextSelectedCultureName {
			get
			{
				return CrossSettings.Current.GetValueOrDefault(NextSelectedCultureKey, _nextselectedCultureDefault);
			}
			set { CrossSettings.Current.AddOrUpdateValue(NextSelectedCultureKey, value); }
		}

        public bool IsCultureSet
        {
            get
            {
                var value = CrossSettings.Current.GetValueOrDefault(SelectedCultureKey, "");
                return !String.IsNullOrEmpty(value);
            }
        }

        public string UserToken
        {
            get
            {
                string token = CrossSettings.Current.GetValueOrDefault(UserTokenKey, _userTokenDefault);
                //#if DEBUG
                //                if (string.IsNullOrEmpty(token))
                //					return "2015120522225570554";
                //#endif
                return token;

            }
            set { CrossSettings.Current.AddOrUpdateValue(UserTokenKey, value); }
        }

	

        public string Language
        {
            get
            {
                if (String.IsNullOrEmpty(SelectedCultureName) || SelectedCultureName.StartsWith("en"))
                    return "eng";
                return "arb";
            }
        }
    }
}
