using System.Xml.Serialization;

namespace AdaaMobile.Models.Request
{
    public class DaypassRequestBParameters
    {
        [XmlElement("reason")]
        public string Reason { get; set; }

    }
}

