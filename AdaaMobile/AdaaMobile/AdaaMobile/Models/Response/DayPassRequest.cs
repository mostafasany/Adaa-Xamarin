using System.Xml.Serialization;

namespace AdaaMobile.Models.Response
{
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]

    public class DayPassRequest
    {
        [XmlElement("Date")]
        public string Date { get; set; }

        [XmlElement("StartTime")]
        public string StartTime { get; set; }

        [XmlElement("EndTime")]
        public string EndTime { get; set; }

        [XmlElement("ReasonType")]
        public string ReasonType { get; set; }
    }
}



