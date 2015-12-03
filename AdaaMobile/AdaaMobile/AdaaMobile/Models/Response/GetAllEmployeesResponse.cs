using System.Xml.Serialization;

namespace AdaaMobile.Models.Response
{
    [XmlRoot("root")]
    public class GetAllEmployeesResponse
    {
        [XmlElement("Item")]
        public Employee[] Employees { get; set; }
    }

}

