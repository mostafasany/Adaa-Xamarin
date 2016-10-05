

namespace AdaaMobile.Models.Request
{
    public class LogIncidentRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Impact { get; set; }
        public string Urgency { get; set; }
        public string Classification { get; set; }
        public string Source { get; set; }
        public string AffectedUser { get; set; }
        public string CreatedByUser { get; set; }
        public string[] FilesNames { get; set; }
        public string[] Files { get; set; }
        public string templateId { get; set; }
        public string[] RA_values { get; set; }
    }
}
