using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.Common;

namespace AdaaMobile.ViewModels
{
    public class AttendanceViewModel : BindableBase
    {

        #region Fields
        #endregion

        #region Properties
        private DateTime _StartDate;

        public DateTime StartDate
        {
            get { return _StartDate; }
            set { SetProperty(ref _StartDate, value); }
        }

        private DateTime _EndDate;

        public DateTime EndDate
        {
            get { return _EndDate; }
            set { SetProperty(ref _EndDate, value); }
        }

        private List<string> _DaysList;

        public List<string> DaysList
        {
            get { return _DaysList; }
            set { SetProperty(ref _DaysList, value); }
        }

        #endregion

        #region Initialization
        public AttendanceViewModel()
        {

            StartDate = new DateTime(2015, 10, 1, 0, 0, 0);
            EndDate = new DateTime(2015, 10, 31, 0, 0, 0);
            var days = new List<string>();
            var currentDate = StartDate;
            while (currentDate <= EndDate)
            {
                days.Add(currentDate.ToString("d MMM"));
                currentDate = currentDate.AddDays(1);
            }
            DaysList = days;
        }
        #endregion

        #region Commands
        #endregion

        #region Methods
        #endregion

    }
}
