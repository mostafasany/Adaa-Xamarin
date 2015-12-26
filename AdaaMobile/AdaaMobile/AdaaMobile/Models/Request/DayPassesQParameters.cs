using QueryExtensions;

namespace AdaaMobile.Models.Request
{
	public class DayPassesQParameters
	{
        [QueryParameter("server")]
        public string Server { get; set; }

        [QueryParameter("url")]
        public string Url
        {
            get { return "?funcname=daypassRequestPending"; }
        }

        [QueryParameter("langid")]
        public string Langid { get; set; }

        [QueryParameter("userToken")]
        public string UserToken { get; set; }
    }
}

