using QueryExtensions;

namespace AdaaMobile.Models.Request
{
    public class DaypassRequestQParameters
    {
        [QueryParameter("server")]
        public string Server { get; set; }

        [QueryParameter("url")]
        public string Url
        {
            get { return "?funcname=daypassRequestNew"; }
        }

        [QueryParameter("langid")]
        public string Langid { get; set; }

        [QueryParameter("userToken")]
        public string UserToken { get; set; }

        [QueryParameter("reasonType")]
        public string ReasonType { get; set; }

        [QueryParameter("willReturn")]
        public string WillReturn { get; set; }

        [QueryParameter("startTime")]
        public string StartTime { get; set; }

        [QueryParameter("endTime")]
        public string EndTime { get; set; }

    }
}

