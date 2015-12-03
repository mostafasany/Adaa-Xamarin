using System.Xml.Serialization;

namespace AdaaMobile.Models.Response
{
    [System.Xml.Serialization.XmlRoot(ElementName = "root", Namespace = "", IsNullable = false)]
    public class AttendanceException
    {
        [XmlElement("msg")]
        public string Msg { get; set; }

        [XmlElement("Late")]
        public string Late { get; set; }

        [XmlElement("Remaining")]
        public string Remaining { get; set; }
    }
}

