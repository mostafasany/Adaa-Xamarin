using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.Common;
using AdaaMobile.Models.Response;

namespace AdaaMobile.Models
{
    public class ExceptionDayWrapper : DayWrapper
    {
        public AttendanceException Details { get; set; }

        public ExceptionDayWrapper()
        {

        }
		protected ExceptionDayWrapper(DateTime dateTime, AttendanceException details) : base(dateTime, false)
        {
            Details = details;
        }

        public static ExceptionDayWrapper Wrap(AttendanceException attException)
        {
            if (attException == null) throw new ArgumentNullException("attException");
            if (string.IsNullOrWhiteSpace(attException.Date)) throw new ArgumentNullException("attException.Date");
            if (!string.IsNullOrWhiteSpace(attException.Message))
            {
                //if (string.IsNullOrEmpty(attException.Late)) attException.Late = attException.Message;
                //if (string.IsNullOrEmpty(attException.Remaining)) attException.Remaining = attException.Message;
                //if (string.IsNullOrEmpty(attException.Duration)) attException.Remaining = attException.Message;
                if (string.IsNullOrEmpty(attException.FirstSeenLoc)) attException.FirstSeenLoc = attException.Message;
                if (string.IsNullOrEmpty(attException.LastSeenLoc)) attException.LastSeenLoc = attException.Message;
            }
            var wrapper = new ExceptionDayWrapper(DateTime.Parse(attException.Date, CultureInfo.InvariantCulture), attException);
            return wrapper;
        }
    }
}
