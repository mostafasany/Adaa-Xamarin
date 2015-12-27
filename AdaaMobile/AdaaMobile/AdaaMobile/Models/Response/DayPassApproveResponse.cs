using System.Xml.Serialization;

namespace AdaaMobile.Models.Response
{
    [XmlRoot("root", IsNullable = true)]
    public class DayPassApproveResponse
	{
		public DayPassApproveResponse ()
		{
		}

        [XmlElement("msg")]
        public string Message { get; set; }
    }
}

