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
        [XmlElement("Equipments")]
        public string Equipments { get; set; }

        [XmlElement("ServiceDetails")]
        public string Servicedetails { get; set; }

        [XmlElement("Location")]
        public string Location { get; set; }

        [XmlElement("Room")]
        public string Room { get; set; }

        [XmlElement("priority")]
        public string Priority { get; set; }

        [XmlElement("requestcomments")]
        public string Requestcomments { get; set; }

    }

    //    <equipments> [Text  EX: ( [equipment_id]#;[title]%; )] </equipments >
    //    <servicedetails> [Text] </servicedetails>
    //    <location> [Text] </location>
    //    <room> [Text] </room>  
    //    <priority> [Normal / Medium / Urgent] </priority> 
    //   <requestcomments> [Comments] </requestcomments> 
}
