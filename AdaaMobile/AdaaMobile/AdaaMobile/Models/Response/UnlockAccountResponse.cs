using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AdaaMobile.Models.Response
{

    [XmlRoot("root")]
    public class UnlockAccountResponse
    {
        [XmlElement("msg")]
        public string Message { get; set; }
    }
}
