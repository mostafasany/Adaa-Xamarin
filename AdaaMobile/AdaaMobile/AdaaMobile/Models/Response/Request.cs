using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AdaaMobile.Models.Response
{
    public class Request
    {
        [XmlElement("title")]
        public string title
        {
            get;
            set;
        }

        [XmlElement("service")]
        public string service
        {
            get;
            set;
        }

        [XmlElement("created")]
        public string created
        {
            get;
            set;
        }

        [XmlElement("assigned")]
        public string assigned
        {
            get;
            set;
        }

        [XmlElement("status")]
        public string status
        {
            get;
            set;
        }

        [XmlElement("url")]
        public string url
        {
            get;
            set;
        }
    }
}
