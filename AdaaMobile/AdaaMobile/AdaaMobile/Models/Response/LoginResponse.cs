using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AdaaMobile.Models.Response
{
    [XmlRoot("root")]
    public class LoginResponse
    {
        [XmlElement("status")]
        public string Status { get; set; }

		[XmlElement("msg")]
		public string Message{ get; set;}


		[XmlElement("token")]
		public string UserToken { get; set; }
    }
}
