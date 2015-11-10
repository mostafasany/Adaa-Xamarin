using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AdaaMobile.Models.Request
{
    [XmlRoot("root")]
    public class LoginBodyParamters
    {
        [XmlElement("username")]
        public string UserName { get; set; }
        [XmlElement("password")]
        public string Password { get; set; }
    }
}
