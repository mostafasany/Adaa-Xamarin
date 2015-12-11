using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AdaaMobile.Models.Response
{
    [XmlRoot("root", Namespace = "", IsNullable = false)]
    public class ExceptionsResponse
    {

        [XmlElement("item")]
        public AttendanceException[] Days { get; set; }

    }

}
