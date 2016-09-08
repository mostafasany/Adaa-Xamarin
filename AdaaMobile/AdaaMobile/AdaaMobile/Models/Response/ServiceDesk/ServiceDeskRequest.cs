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
		public string Title { get; set; }
		public string Type { get; set; }
        public string Description { get; set; }
		public DateTime CreatedDate { get; set; }
		private string _AreaID;
        public string AreaID {
			get
			{
				return _AreaID;
			}	

			set
			{
				_AreaID = value;
				ClassificationID = _AreaID;
				Type = "Incidents";
			} 
		}
		private string _Area;
		public string Area
		{
			get
			{
				return _Area;
			}
			set
			{
				_Area = value;
				Classification = _Area;
				Type = "Service Request";
			}
		}
		private string _Classification;
        public string Classification {
			get
			{
				return _Classification;
			}
			set
			{

				_Classification = value;
				Type = "Incidents";

			} 
		}
        public string ClassificationID { get; set; }
        public string Status { get; set; }
        
 
    }
}
