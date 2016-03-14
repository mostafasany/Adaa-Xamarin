using QueryExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AdaaMobile.Models.Request
{
[XmlRoot("root")]
    public class SaveDriverRequestBParameters
    {

			[QueryParameter("requesttype")]
			public string requesttype { get; set; }

			[QueryParameter("reason")]
			public string reason { get; set; }

			[QueryParameter("requestdate")]
			public string requestdate { get; set; }

			[QueryParameter("source")]
			public string source { get; set; }

			[QueryParameter("destination")]
			public string destination { get; set; }

			[QueryParameter("priority")]
			public string priority { get; set; }

			[QueryParameter("requestcomments")]
			public string requestcomments { get; set; }

       
    }

    //<requesttype> [Work / Personal]</requesttype>
    //<reason> [Delivering Correspondence / Receiving Correspondence / Other(Fill -in] </reason>
    //<requestdate> [DateTime] </requestdate>
    //<source> [Client Name] </source> 
    //<destination> [Client Name] </destination> 
    //<priority> [Normal / Medium / Urgent] </priority> 
    //<requestcomments> [Comments] </requestcomments> 
}
