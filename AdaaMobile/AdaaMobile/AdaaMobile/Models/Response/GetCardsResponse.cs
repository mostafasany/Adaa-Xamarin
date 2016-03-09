using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AdaaMobile.Models.Response
{
    [XmlRoot("root")]
    public class GetCardsResponse
    {
        [XmlElement("msg")]
        public string Message { get; set; }

        [XmlElement("title")]

        public string title
        {
            get; set;
        }

        [XmlElement("id")]
        public string id
        {
            get;
            set;
        }

        [XmlElement("img")]
        public string img
        {
            get;
            set;
        }
    }



}
