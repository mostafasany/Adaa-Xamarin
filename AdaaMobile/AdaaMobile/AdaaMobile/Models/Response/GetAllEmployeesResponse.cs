using System.Xml.Serialization;

namespace AdaaMobile.Models.Response
{
    [XmlRoot("root")]
    public class GetAllEmployeesResponse
    {
        [XmlElement("msg")]
        public string Message { get; set; }

        [XmlElement("Item")]
        public Employee[] Employees { get; set; }
    }

}

