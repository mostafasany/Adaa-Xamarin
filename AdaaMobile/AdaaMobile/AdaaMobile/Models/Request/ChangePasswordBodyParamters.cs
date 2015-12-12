using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AdaaMobile.Models.Request
{
    [XmlRoot("root")]
    public class ChangePasswordBodyParamters
    {

        [XmlElement("password")]
        public string Password { get; set; }
    }
}
