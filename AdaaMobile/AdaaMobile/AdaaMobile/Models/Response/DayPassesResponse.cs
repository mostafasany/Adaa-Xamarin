using System.Xml.Serialization;

namespace AdaaMobile.Models.Response
{
	public class DayPassesResponse
	{
		public DayPassesResponse ()
		{
		}

        [XmlElement("msg")]
        public string Message { get; set; }
    }
}

