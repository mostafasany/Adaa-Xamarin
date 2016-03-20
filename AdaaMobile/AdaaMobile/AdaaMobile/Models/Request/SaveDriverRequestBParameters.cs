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

			[XmlElement("requesttype")]
			public string Requesttype { get; set; }

			[XmlElement("reason")]
			public string Reason { get; set; }

			[XmlElement("requestdate")]
			public string Requestdate { get; set; }

			[XmlElement("source")]
			public string Source { get; set; }

			[XmlElement("destination")]
			public string Destination { get; set; }

			[XmlElement("priority")]
			public string Priority { get; set; }

			[XmlElement("requestcomments")]
			public string Requestcomments { get; set; }

       
    }

    //<requesttype> [Work / Personal]</requesttype>
    //<reason> [Delivering Correspondence / Receiving Correspondence / Other(Fill -in] </reason>
    //<requestdate> [DateTime] </requestdate>
    //<source> [Client Name] </source> 
    //<destination> [Client Name] </destination> 
    //<priority> [Normal / Medium / Urgent] </priority> 
    //<requestcomments> [Comments] </requestcomments> 
}
