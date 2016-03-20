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
    public class SaveOfficeMaintenanceRequestBParameters
    {
        [QueryParameter("equipments")]
        public string Equipments { get; set; }

        [QueryParameter("servicedetails")]
        public string Servicedetails { get; set; }

        [QueryParameter("location")]
        public string Location { get; set; }

        [QueryParameter("room")]
        public string Room { get; set; }

        [QueryParameter("priority")]
        public string Priority { get; set; }

        [QueryParameter("requestcomments")]
        public string Requestcomments { get; set; }

    }

    //    <equipments> [Text  EX: ( [equipment_id]#;[title]%; )] </equipments >
    //    <servicedetails> [Text] </servicedetails>
    //    <location> [Text] </location>
    //    <room> [Text] </room>  
    //    <priority> [Normal / Medium / Urgent] </priority> 
    //   <requestcomments> [Comments] </requestcomments> 
}
