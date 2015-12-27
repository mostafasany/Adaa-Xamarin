using System.Xml.Serialization;

namespace AdaaMobile.Models.Response
{
    /// <remarks/>
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]

    public class DayPassTask
    {
        [XmlElement("UserName")]
        public string UserName { get; set; }

        [XmlElement("UserID")]
        public string UserId { get; set; }

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
