using QueryExtensions;

namespace AdaaMobile.Models.Request
{
    public class ExceptionQParamters
    {
        [QueryParameter("server")]
        public string Server { get; set; }

        [QueryParameter("url")]
        public string Url
        {
            get { return "funcname=getAttendanceExceptions"; }//TODO:Change after backend Rename
        }

        [QueryParameter("langid")]
        public string Langid { get; set; }

        [QueryParameter("userToken")]
        public string UserToken { get; set; }

        [QueryParameter("date")]
        public string Date { get; set; }

    }
}

