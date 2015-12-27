using System.Xml.Serialization;

namespace AdaaMobile.Models.Response
{
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class Delegation
    {
        [XmlElement("ManagerName")]
        public string ManagerName { get; set; }

        [XmlElement("ManagerID")]
        public string ManagerId { get; set; }

        [XmlElement("DelegateName")]
        public string DelegateName { get; set; }

        [XmlElement("DelegateID")]
        public string DelegateId { get; set; }

        [XmlElement("SubordinateName")]
        public string SubordinateName { get; set; }

        [XmlElement("SubordinateID")]
        public string SubordinateId { get; set; }
    }
}

