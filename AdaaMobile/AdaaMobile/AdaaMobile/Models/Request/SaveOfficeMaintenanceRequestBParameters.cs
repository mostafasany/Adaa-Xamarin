using QueryExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaaMobile.Models.Request
{
    public class SaveOfficeMaintenanceRequestBParameters
    {
        [QueryParameter("equipments")]
        public string equipments { get; set; }

        [QueryParameter("servicedetails")]
        public string servicedetails { get; set; }

        [QueryParameter("location")]
        public string location { get; set; }

        [QueryParameter("room")]
        public string room { get; set; }

        [QueryParameter("priority")]
        public string priority { get; set; }

        [QueryParameter("requestcomments")]
        public string requestcomments { get; set; }

    }

    //    <equipments> [Text  EX: ( [equipment_id]#;[title]%; )] </equipments >
    //    <servicedetails> [Text] </servicedetails>
    //    <location> [Text] </location>
    //    <room> [Text] </room>  
    //    <priority> [Normal / Medium / Urgent] </priority> 
    //   <requestcomments> [Comments] </requestcomments> 
}
