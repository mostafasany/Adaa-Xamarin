using QueryExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaaMobile.Models.Request
{
    public class GetCardsQParameters
    {
        [QueryParameter("server")]
        public string Server { get; set; }

        [QueryParameter("url")]
        public string Url
        {
            get { return "?funcname=getCards"; }
        }

        [QueryParameter("langid")]
        public string Langid { get; set; }

        [QueryParameter("userToken")]
        public string UserToken { get; set; }

//        [QueryParameter("mid")]
//        public string LocationId { get; set; }

    }
}
