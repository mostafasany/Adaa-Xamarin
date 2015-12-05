using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QueryExtensions;

namespace AdaaMobile.Models.Request
{
    public class OtherProfileQParameters
    {
        [QueryParameter("server")]
        public string Server { get; set; }

        [QueryParameter("url")]
        public string Url {
            get { return "funcname=getUserProfile"; }
        }

        [QueryParameter("langid")]
        public string Langid { get; set; }

        [QueryParameter("userToken")]
        public string UserToken { get; set; }

        [QueryParameter("empID")]
        public string EmpId { get; set; }
    }
}
