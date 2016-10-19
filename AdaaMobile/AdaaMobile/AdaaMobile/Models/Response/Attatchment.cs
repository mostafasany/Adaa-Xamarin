namespace AdaaMobile.Models.Response
{
    public class AttatchmentRoot
    {
        public Attatchments result { get; set; }
        public string Exception { get; set; }
    }

    public class Attatchments
    {
        public Attatchment[] Table1 { get; set; }
    }

    public class Attatchment
    {
        public string SRID { get; set; }
        public string Value { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string BlobId { get; set; }
        public string AttachmentID { get; set; }
        public int AttachmentSize { get; set; }
    }

}
