using System;

namespace AdaaMobile.Models.Response
{
    public class Week
    {
        public string WeekText { get; set; }
        public int WeekNumber { get; set; }
        public DateTime WeekStart { get; set; }
        public DateTime WeekEnd { get; set; }
    }
}
