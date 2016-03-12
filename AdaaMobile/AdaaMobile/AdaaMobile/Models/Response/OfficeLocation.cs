using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AdaaMobile.Models.Response
{
    public class OfficeLocation
    {
        [XmlElement("id")]
        public string Id
        {
            get;
            set;
        }

        [XmlElement("title")]
        public string Title
        {
            get;
            set;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
