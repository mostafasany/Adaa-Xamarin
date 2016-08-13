using System;

namespace AdaaMobile.Models.Response
{
    public class Week
    {
        public string WeekText { get; set; }
        public int WeekNumber
        {
            get
            {
                string[] splitedWeek = WeekText.Split(':');
                if (splitedWeek != null)
                {
                    var weekArray = splitedWeek[0].Split("Week".ToCharArray());
                    if (weekArray != null)
                        return int.Parse(weekArray[1]);
                }

                return 1;
            }
        }

        public DateTime WeekStart
        {
            get
            {
                string[] splitedWeek = WeekText.Split(':');
                if (splitedWeek != null)
                {
                    var weekArray = splitedWeek[1].Split("To".ToCharArray());
                    if (weekArray != null)
                        return DateTime.Parse(weekArray[0].Trim());
                }
                return DateTime.Now;
            }
        }

        public DateTime WeekEnd
        {
            get
            {
                string[] splitedWeek = WeekText.Split(':');
                if (splitedWeek != null)
                {
                    var weekArray = splitedWeek[1].Split("To".ToCharArray());
                    if (weekArray != null)
                        return DateTime.Parse(weekArray[1].Trim());
                }
                return DateTime.Now;
            }
        }
    }
}
