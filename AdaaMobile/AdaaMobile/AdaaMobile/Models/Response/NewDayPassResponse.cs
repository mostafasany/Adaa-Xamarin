using System.Xml.Serialization;

namespace AdaaMobile.Models.Response
{
	public class NewDayPassResponse
	{
        [XmlElement("msg")]
        public string Message { get; set; }
    }
}

