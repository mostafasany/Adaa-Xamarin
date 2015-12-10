using System.Xml.Serialization;

namespace AdaaMobile.Models.Response
{

    [System.Xml.Serialization.XmlRoot(ElementName = "root", Namespace = "", IsNullable = false)]
    public partial class Attendance
    {
        [XmlElement("msg")]
        public string Message { get; set; }

        [XmlElement("FirstSeen")]
        public string FirstSeen { get; set; }

        [XmlElement("LastSeen")]
        public string LastSeen { get; set; }

        [XmlElement("Duration")]
        public string Duration { get; set; }

        [XmlElement("FirstSeenLoc")]
        public string FirstSeenLoc { get; set; }

        [XmlElement("LastSeenLoc")]
        public string LastSeenLoc { get; set; }
    }


}

