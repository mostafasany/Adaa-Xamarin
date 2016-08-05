using System.Xml.Serialization;

namespace AdaaMobile.Models.Response
{

    [System.Xml.Serialization.XmlRoot(ElementName = "root", Namespace = "", IsNullable = false)]
    public partial class Task
    {
        [XmlElement("ID")]
        public string Id { get; set; }

		[XmlElement("ProcedureName")]
		public string Name { get; set; }

		[XmlElement("TaskFullURL")]
        public string URL { get; set; }
       
    }


}

