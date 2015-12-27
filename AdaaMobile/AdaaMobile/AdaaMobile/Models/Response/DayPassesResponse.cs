using System.Xml.Serialization;

namespace AdaaMobile.Models.Response
{
    [XmlRoot("root", IsNullable = true)]
    public class DayPassesResponse
    {

        [XmlElement("msg")]
        public string Message { get; set; }

        [XmlElement("Item")]
        public DayPassRequest[] DayPassList { get; set; }
    }
}

