using System;
using QueryExtensions;

namespace AdaaMobile.Models.Request
{
    public class ExceptionDetailsQParamters
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

        [QueryParameter("date", Format = "d_M_yyyy")]
        public DateTime Date { get; set; }

    }
}

