using QueryExtensions;

namespace AdaaMobile.Models.Request
{
	public class DaypassApproveQParameters
	{
        [QueryParameter("server")]
        public string Server { get; set; }

        [QueryParameter("url")]
        public string Url
        {
            get { return "?funcname=daypassTaskApprove"; }
        }

        [QueryParameter("langid")]
        public string Langid { get; set; }

        [QueryParameter("userToken")]
        public string UserToken { get; set; }


        [QueryParameter("subID")]
        public string SubID { get; set; }

        [QueryParameter("date")]
        public string Date { get; set; }


        [QueryParameter("startTime")]
        public string StartTime { get; set; }

        [QueryParameter("approve")]
        public string approve { get; set; }

        

    }
}

