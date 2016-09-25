using System;

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
		public DateTime CreatedDate { get; set; }
		public string CreatedDateFormated { get { return CreatedDate.Date.ToString("dd MMMMM yyyy"); } }
		public string Id { get; set; }
		public string RA_Guid { get; set; }
		public string Title { get; set; }
		public string Reviewer { get; set; }
		public string ReviewerId { get; set; }
		public string Decision { get; set; }
		public object DecisionDate { get; set; }
		//public string DecisionDateFormated { get { return DecisionDate.Date.ToString("dd MMMMM yyyy"); } }

		public string StatusID { get; set; }
		public string comment { get; set; }
		public string Status { get; set; }
		public string Status_AR { get; set; }
		public string Decision_AR { get; set; }
		public string Status_FR { get; set; }
		public string Decision_FR { get; set; }
		public object CreatedDate_AR { get; set; }
		//public string CreatedDate_ARFormated { get { return CreatedDate_AR.Date.ToString("dd MMMMM yyyy"); } }

		public object DecisionDate_AR { get; set; }
		//public string DecisionDate_ARFormated { get { return DecisionDate_AR.Date.ToString("dd MMMMM yyyy"); } }

		private string _Parent_ID;
		public string Parent_ID
		{
			get
			{
				return _Parent_ID;
			}
			set
			{
				_Parent_ID = value;
				if (_Parent_ID.Contains("SR"))
				{
					Type = "Service Request";

				}
				else if (_Parent_ID.Contains("CR"))
				{
					Type = "Change Request";
				}
			}
		}
		public string Type { get; set; }

		public string CreatedByUserID { get; set; }
		public string CreatedBy { get; set; }
		public string DisplayName { get; set; }
		public object Description { get; set; }
		public int ActivityOrder { get; set; }
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
