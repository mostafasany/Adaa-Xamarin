using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AdaaMobile.Models.Response
{
    public class Client
    {


        [XmlElement("id")]
        public string id
        {
            get;
            set;
        }

        [XmlElement("title")]
        public string title
        {
            get;
            set;
        }

    }
}
