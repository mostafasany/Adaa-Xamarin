

namespace AdaaMobile.Models.Request
{
    public class NewServiceRequest
    {
        //public string titleField { get; set; }
        //public string descriptionField { get; set; }
        //public string calssificationIDField { get; set; }
        //public string templateIDField { get; set; }
        //public string rA_ValuesField { get; set; }
        //public string filesNamesField { get; set; }
        //public string filesBytesField { get; set; }
        //public string affectedUserField { get; set; }
        //public string pocoStatusField { get; set; }
        //public string createdByUserField { get; set; }

		public string titleField { get; set; }
		public string descriptionField { get; set; }
		public string calssificationIDField { get; set; }
		public string templateIDField { get; set; }
		public string[] filesNamesField { get; set; }
		public string[] filesBytesField { get; set; }
		public string pocoStatusField { get; set; }
		public string[] rA_ValuesField { get; set; }
		public string affectedUserField { get; set; }
		public string createdByUserField { get; set; }
    }
}
