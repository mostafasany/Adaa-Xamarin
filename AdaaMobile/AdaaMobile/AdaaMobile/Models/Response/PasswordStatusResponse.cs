using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AdaaMobile.Models.Response
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [XmlRoot("root", Namespace = "", IsNullable = false)]
    public class PasswordStatusResponse
    {
        [XmlElement("msg")]
        public string Message { get; set; }
    }
}
