using System;
using System.Globalization;
using System.Xml.Serialization;
using AdaaMobile.Common;
using AdaaMobile.Strings;

namespace AdaaMobile.Models.Response
{
    /// <remarks/>
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]

    public class DayPassTask:BindableBase
    {
        [XmlElement("UserName")]
        public string UserName { get; set; }

        [XmlElement("UserID")]
        public string UserId { get; set; }

        [XmlElement("Date")]
        public string Date { get; set; }

        [XmlElement("StartTime")]
        public string StartTime { get; set; }

        [XmlElement("EndTime")]
        public string EndTime { get; set; }

        [XmlElement("ReasonType")]
        public string ReasonType { get; set; }

		[XmlElement("ReasonText")]
		public string ReasonText { get; set; }

		[XmlElement("WillReturn")]
		public string WillReturn { get; set; }
	

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
                catch (Exception )
                {

                    return AppResources.EmptyPlaceHolder;
                }
            }
        }

        /// <summary>
        /// Will be assigned from another request
        /// </summary>
        private string _departement;
        public string Department
        {
            get { return _departement; }
            set { SetProperty(ref _departement, value); }
        }

    }


}
