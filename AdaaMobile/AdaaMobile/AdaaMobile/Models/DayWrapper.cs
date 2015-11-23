using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.Common;

namespace AdaaMobile.Models
{
    public class DayWrapper:BindableBase
    {
        public DateTime Date { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }


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
