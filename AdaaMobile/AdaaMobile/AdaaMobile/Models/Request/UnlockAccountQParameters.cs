using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QueryExtensions;

namespace AdaaMobile.Models.Request
{
    public class UnlockAccountQParameters
    {
        [QueryParameter("server")]
        public string Server { get; set; }

        [QueryParameter("url")]
        public string Url { get { return "?funcname=unlockAccount"; } }

        [QueryParameter("langid")]
        public string Langid { get; set; }

        [QueryParameter("userToken")]
        public string UserToken { get; set; }
    }
}
