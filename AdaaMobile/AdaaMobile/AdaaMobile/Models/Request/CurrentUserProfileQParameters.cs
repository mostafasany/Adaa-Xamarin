using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using QueryExtensions;

namespace AdaaMobile.Models.Request
{
    public class CurrentUserProfileQParameters
    {
        [QueryParameter("funcname")]
        public string FuncName
        {
            get { return "getCurrentUserProfile"; }
        }
        [QueryParameter("userToken")]
        public string UserToken { get; set; }
        [QueryParameter("Langid")]
        public string LangId { get; set; }
    }
}
