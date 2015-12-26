using System.Xml.Serialization;

namespace AdaaMobile.Models.Response
{
    public class DayPassesResponse
    {

        [XmlElement("msg")]
        public string Message { get; set; }


        [XmlElement("Item")]
        public DayPassRequest[] DayPassList { get; set; }
    }
}

