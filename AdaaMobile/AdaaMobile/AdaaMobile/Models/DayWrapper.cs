using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaaMobile.Models
{
    public class DayWrapper
    {
        public DateTime Date { get; set; }

        public DayWrapper()
        {

        }
        public DayWrapper(DateTime date)
        {
            Date = date;
        }

        public int Day
        {
            get { return Date.Day; }
        }

        public string AbbrMonth
        {
            get { return Date.ToString("MMM"); }
        }

        public string AbbrWeek
        {
            get { return Date.ToString("ddd"); }
        }
    }
}
