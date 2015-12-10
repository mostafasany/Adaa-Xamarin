using System.Xml.Serialization;

namespace AdaaMobile.Models.Response
{
	public class DayPassTasksResponse
	{
		public DayPassTasksResponse ()
		{
		}

        [XmlElement("msg")]
        public string Message { get; set; }
    }
}

