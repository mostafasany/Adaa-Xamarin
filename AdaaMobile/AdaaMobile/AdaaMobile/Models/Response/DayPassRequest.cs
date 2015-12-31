using System;
using System.Globalization;
using System.Xml.Serialization;
using AdaaMobile.Strings;

namespace AdaaMobile.Models.Response
{
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]

    public class DayPassRequest
    {
        [XmlElement("Date")]
        public string Date { get; set; }

        [XmlElement("StartTime")]
        public string StartTime { get; set; }

        [XmlElement("EndTime")]
        public string EndTime { get; set; }

        public string Duration
        {
            get
            {
                try
                {
                    TimeSpan startSpan;
                    TimeSpan.TryParse(StartTime, CultureInfo.InvariantCulture, out startSpan);
                    TimeSpan endSpan;
                    TimeSpan.TryParse(EndTime, CultureInfo.InvariantCulture, out endSpan);
                    var duration = (endSpan - startSpan);
                    if (duration.TotalSeconds > 0)
                        return string.Format("{0:h\\:mm} hrs", duration);
                    return AppResources.EmptyPlaceHolder;
                }
                catch (Exception ex)
                {

                    return AppResources.EmptyPlaceHolder;
                }
            }
        }

        [XmlElement("ReasonType")]
        public string ReasonType { get; set; }
    }
}



