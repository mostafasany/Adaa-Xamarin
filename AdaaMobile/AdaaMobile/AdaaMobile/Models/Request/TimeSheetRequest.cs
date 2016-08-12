

namespace AdaaMobile.Models.Request
{
    public class TimeSheetRequest
    {
        public int AssignmentID { get; set; }
        public int TaskID { get; set; }
        public string Sunday { get; set; }
        public string SundayComment { get; set; }
        public string Monday { get; set; }
        public string MondayComment { get; set; }
        public string Tuesday { get; set; }
        public string TuesdayComment { get; set; }
        public string Wednesday { get; set; }
        public string WednesdayComment { get; set; }
        public string Thursday { get; set; }
        public string ThursdayComment { get; set; }
    }
}
