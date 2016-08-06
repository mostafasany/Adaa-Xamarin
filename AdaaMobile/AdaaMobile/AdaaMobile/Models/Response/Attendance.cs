using System.Xml.Serialization;

namespace AdaaMobile.Models.Response
{

    [System.Xml.Serialization.XmlRoot(ElementName = "root", Namespace = "", IsNullable = false)]
	public partial class Assignment
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Date { get; set; }

        public string Status { get; set; }

    }


}

