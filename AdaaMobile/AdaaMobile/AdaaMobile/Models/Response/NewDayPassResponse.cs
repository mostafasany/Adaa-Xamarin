using System.Xml.Serialization;

namespace AdaaMobile.Models.Response
{
    [XmlRoot("root", IsNullable = true)]
    public class NewDayPassResponse
	{
        [XmlElement("msg")]
        public string Message { get; set; }
    }
}

