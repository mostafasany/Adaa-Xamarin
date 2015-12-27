using System.Xml.Serialization;

namespace AdaaMobile.Models.Response
{
	[XmlRoot("root")]
	public class DayPassApproveResponse
	{
		public DayPassApproveResponse ()
		{
		}

        [XmlElement("msg")]
        public string Message { get; set; }
    }
}

