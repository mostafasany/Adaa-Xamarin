namespace AdaaMobile.Models.Response
{

    [System.Xml.Serialization.XmlRoot(ElementName = "root", Namespace = "", IsNullable = false)]
    public partial class PendingTask
    {
        public string Id { get; set; }

        public string IdWithReference
        {
            get
            {
                return "Reference " + Id;
            }
        }

        public string ProcedureName { get; set; }

        public string TaskFullURL { get; set; }
    }


}

