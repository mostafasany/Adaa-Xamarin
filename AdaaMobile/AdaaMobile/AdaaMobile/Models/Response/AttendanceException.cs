using System.Xml.Serialization;

namespace AdaaMobile.Models.Response
{
    //Used in ExceptionsResponse
    [System.Xml.Serialization.XmlRoot(ElementName = "item", Namespace = "", IsNullable = false)]
    public class AttendanceException
    {
        [XmlElement("Date")]
        public string Date { get; set; }

        [XmlElement("msg")]
        public string Message { get; set; }

        [XmlElement("Late")]
        public string Late { get; set; }

        [XmlElement("Remaining")]
        public string Remaining { get; set; }

        [XmlElement("Duration")]
        public string Duration { get; set; }

        [XmlElement("FirstSeenLoc")]
        public string FirstSeenLoc { get; set; }

        [XmlElement("LastSeenLoc")]
        public string LastSeenLoc { get; set; }
        
    }
}

