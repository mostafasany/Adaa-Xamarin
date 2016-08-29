namespace AdaaMobile.Models.Response
{

    public class ServiceDeskCases
    {
        public ServiceDeskCasesResult result { get; set; }
        public string Exception { get; set; }
    }

    public class ServiceDeskCasesResult
    {
        public ServiceDeskCase[] Table1 { get; set; }
    }

    public class ServiceDeskCase
    {
        public int MustVote { get; set; }
        public string CreatedDate { get; set; }
        public string Id { get; set; }
        public string RA_Guid { get; set; }
        public string Title { get; set; }
        public string Reviewer { get; set; }
        public string ReviewerId { get; set; }
        public string Decision { get; set; }
        public object DecisionDate { get; set; }
        public string StatusID { get; set; }
        public object comment { get; set; }
        public string Status { get; set; }
        public string Status_AR { get; set; }
        public string Decision_AR { get; set; }
        public string Status_FR { get; set; }
        public string Decision_FR { get; set; }
        public string CreatedDate_AR { get; set; }
        public object DecisionDate_AR { get; set; }
        public string Parent_ID { get; set; }
        public string CreatedByUserID { get; set; }
        public string CreatedBy { get; set; }
        public string DisplayName { get; set; }
        public object Description { get; set; }
        public int	  ActivityOrder { get; set; }
        public string ReviewerDomain { get; set; }
        public string AffectedUser { get; set; }
        public string AffectedUserDomain { get; set; }
        public string AffectedUserDispalyName { get; set; }
        public string ReviewerDisplayName { get; set; }
        public string Parent_Title { get; set; }
        public object VotedByUser { get; set; }
        public object VotedByUserDomain { get; set; }
        public string WITYPE { get; set; }
    }

}
