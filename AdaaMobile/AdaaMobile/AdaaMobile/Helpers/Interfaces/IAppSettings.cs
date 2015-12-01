using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaaMobile.Helpers
{
    public interface IAppSettings
    {
        string SelectedCultureName { get; set; }
		string UserToken { get; set;}
    }
}
