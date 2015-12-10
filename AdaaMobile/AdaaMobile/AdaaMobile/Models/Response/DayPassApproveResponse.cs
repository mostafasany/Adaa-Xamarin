using System.Xml.Serialization;

namespace AdaaMobile.Models.Response
{
	public class DayPassApproveResponse
	{
		public DayPassApproveResponse ()
		{
		}

        [XmlElement("msg")]
        public string Message { get; set; }
    }
}

