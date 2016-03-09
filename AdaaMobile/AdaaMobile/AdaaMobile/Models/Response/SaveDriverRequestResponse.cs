using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AdaaMobile.Models.Response
{
    public class SaveDriverRequestResponse
    {
        [XmlElement("msg")]
        public string Message { get; set; }

        [XmlElement("type")]
        public string type { get; set; }
    }
}
