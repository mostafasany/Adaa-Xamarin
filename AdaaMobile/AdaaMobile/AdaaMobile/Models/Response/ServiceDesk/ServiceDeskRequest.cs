using System;

namespace AdaaMobile.Models.Response
{
    public class ServiceDeskRequests
    {
        public ServiceDeskRequestsResult result { get; set; }
        public string Exception { get; set; }
    }

    public class ServiceDeskRequestsResult
    {
        public ServiceDeskRequest[] Table1 { get; set; }
    }

    public class ServiceDeskRequest
    {
        public string ID { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string DisplayName { get; set; }
        public string Impact { get; set; }
        public string ImpactID { get; set; }
        public string Urgency { get; set; }
        public string UrgencyID { get; set; }
        public int Priority { get; set; }
        public string AffectedUser { get; set; }
        public string AffectedUserDomain { get; set; }
        public string Classification { get; set; }
        public string ClassificationID { get; set; }
        public object AssignedTo { get; set; }
        public object AssignedToDomain { get; set; }
        public string Status { get; set; }
        public string StatusID { get; set; }
        public string Source { get; set; }
        public string SourceID { get; set; }
        public object LastModifiedSource { get; set; }
        public object LastModifiedSourceID { get; set; }
        public string TierQueue { get; set; }
        public string TierQueueID { get; set; }
        public object ResolvedDate { get; set; }
        public object ActualDowntimeStartDate { get; set; }
        public object ActualDowntimeEndDate { get; set; }
        public object ActualStartDate { get; set; }
        public object ActualEndDate { get; set; }
        public object ActualWork { get; set; }
        public string BaseManagedEntityId { get; set; }
        public object ActualCost { get; set; }
        public object ClosedDate { get; set; }
        public object ContactMethod { get; set; }
        public bool Escalated { get; set; }
        public object FirstAssignedDate { get; set; }
        public object FirstResponseDate { get; set; }
        public bool HasCreatedKnowledgeArticle { get; set; }
        public object IsDowntime { get; set; }
        public object IsParent { get; set; }
        public string lastmodified { get; set; }
        public object AffecteduserLocation { get; set; }
        public object affecteduserloaction { get; set; }
        public string Title { get; set; }
        public int NumberOFAttachements { get; set; }
        public string CreatedByUserName { get; set; }
        public string CreatedByUserDomain { get; set; }
        public string AffectedUserDispalyName { get; set; }
        public object PowneruserName { get; set; }
    }
}
