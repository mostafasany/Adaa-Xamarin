using QueryExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaaMobile.Models.Request
{
    public class NewDelegationQParameter
    {
        [QueryParameter("server")]
        public string Server { get; set; }

        [QueryParameter("url")]
        public string Url
        {
            get { return "?funcname=delegationRequestNew"; }
        }

        [QueryParameter("langid")]
        public string Langid { get; set; }

        [QueryParameter("userToken")]
        public string UserToken { get; set; }

        [QueryParameter("delegateID")]
        public string DelegateID { get; set; }

        [QueryParameter("subordinateID")]
        public string SubordinateID { get; set; }
    }
}
