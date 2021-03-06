﻿using QueryExtensions;

namespace AdaaMobile.Models.Request
{
	public class DayPassTasksQParameters
	{
        [QueryParameter("server")]
        public string Server { get; set; }

        [QueryParameter("url")]
        public string Url
        {
            get { return "?funcname=daypassTasksShow"; }
        }

        [QueryParameter("langid")]
        public string Langid { get; set; }

        [QueryParameter("userToken")]
        public string UserToken { get; set; }

    }
}

