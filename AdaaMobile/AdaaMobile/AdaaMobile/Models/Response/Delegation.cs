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
        public ushort ManagerId { get; set; }

        [XmlElement("DelegateName")]
        public string DelegateName { get; set; }

        [XmlElement("DelegateID")]
        public uint DelegateId { get; set; }

        [XmlElement("SubordinateName")]
        public string SubordinateName { get; set; }

        [XmlElement("SubordinateID")]
        public ushort SubordinateId { get; set; }
    }
}

