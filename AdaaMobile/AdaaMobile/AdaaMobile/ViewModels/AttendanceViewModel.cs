using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.Common;
using AdaaMobile.Models;

namespace AdaaMobile.ViewModels
{
    public class AttendanceViewModel : BindableBase
    {
        private const int LimitRangeInDays = 2 * 365;

        #region Fields
        #endregion

        #region Properties
        private DateTime _startDate;
        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                if (SetProperty(ref _startDate, value))
                {
                    PopulateDays();
                }
            }
        }

        private DateTime _endDate;

        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                if (SetProperty(ref _endDate, value))
                {
                    PopulateDays();
                }
            }
        }

        private DateTime _minimumDate = DateTime.Now.Subtract(TimeSpan.FromDays(LimitRangeInDays));
        public DateTime MinimumDate
        {
            get { return _minimumDate; }
            set { _minimumDate = value; }
        }

        private List<DayWrapper> _daysList;
        public List<DayWrapper> DaysList
        {
            get { return _daysList; }
            set { SetProperty(ref _daysList, value); }
        }

        private DayWrapper _selectedDay;
        public DayWrapper SelectedDay
        {
            get { return _selectedDay; }
            set { SetProperty(ref _selectedDay, value); }
        }


        #endregion

        #region Initialization
        public AttendanceViewModel()
        {

            _startDate = new DateTime(2015, 10, 1, 0, 0, 0);
            _endDate = new DateTime(2015, 10, 31, 0, 0, 0);
            PopulateDays();
        }

        private void PopulateDays()
        {
            var days = new List<DayWrapper>();
            var currentDate = StartDate;
            var endDate = EndDate;
            //Limit large unneccessary range if any
            // if ((EndDate - StartDate).Days > LimitRangeInDays) endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 0, 0, 0);
            while (currentDate <= endDate)
            {
                days.Add(new DayWrapper(currentDate));
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
