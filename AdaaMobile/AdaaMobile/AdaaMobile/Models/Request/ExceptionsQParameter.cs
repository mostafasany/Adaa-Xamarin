using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QueryExtensions;

namespace AdaaMobile.Models.Request
{
    public class ExceptionsQParameter
    {
        [QueryParameter("server")]
        public string Server { get; set; }

        [QueryParameter("url")]
        public string Url
        {
            get { return "?funcname=getAttendanceExceptions"; }
        }

        [QueryParameter("langid")]
        public string Langid { get; set; }

        [QueryParameter("userToken")]
        public string UserToken { get; set; }

        [QueryParameter("startDate", Format = "d_M_yyyy")]
        public DateTime StartDate { get; set; }

        [QueryParameter("endDate", Format = "d_M_yyyy")]
        public DateTime EndDate { get; set; }

        [QueryParameter("subordinateID", AllowEmpty = true)]
        public string subordinateID { get; set; }
    }
}
