using System;
using AdaaMobile.Common;

namespace AdaaMobile.Models
{
    public class DayWrapper : BindableBase
    {
        public DateTime Date { get; set; }

        public bool IsDummy { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }


        public DayWrapper()
        {

        }


        public DayWrapper(DateTime date, bool isDummy)
        {
            Date = date;
            IsDummy = isDummy;
        }

        public string Day
        {
            get
            {
                if (IsDummy)
                    return " ";
                return Date.Day.ToString();
            }
        }

        public string AbbrMonth
        {
            get
            {
                if (IsDummy)
                    return " ";
                return Date.ToString("MMM");
            }
        }

        public string AbbrWeek
        {
            get
            {
                if (IsDummy)
                    return " ";
                return Date.ToString("ddd");
            }
        }
    }
}
