using System.Xml.Serialization;

namespace AdaaMobile.Models.Response
{

    [System.Xml.Serialization.XmlRoot(ElementName = "root", Namespace = "", IsNullable = false)]
	public partial class Assignment
    {
        [XmlElement("ID")]
        public string Id { get; set; }

        [XmlElement("Title")]
        public string Title { get; set; }
        
    }


}

