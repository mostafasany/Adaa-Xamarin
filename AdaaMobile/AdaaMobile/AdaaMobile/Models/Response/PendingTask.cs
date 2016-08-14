using AdaaMobile.Strings;
using AdaaMobile.ViewModels;
using System;

namespace AdaaMobile.Models.Response
{

    [System.Xml.Serialization.XmlRoot(ElementName = "root", Namespace = "", IsNullable = false)]
    public partial class PendingTask
    {
        public string Id { get; set; }

        public string IdWithReference
        {
            get
            {
                return AppResources.TimeSheet_Refrence + " #" + Id;
            }
        }

        public string ProcedureName { get; set; }

        public DateTime TaskDate { get; set; }
        public string TaskDateFormated { get { return TaskDate.Date.ToString("yyyy MMMMM dd"); } }

        public string TaskFullURL { get; set; }

        public string StatusName
        {
            get
            {
                if (Locator.Default.AppSettings.SelectedCultureName.Contains("ar"))
                {
                    return StatusNameAR;
                }
                else
                {
                    return StatusNameEN;
                }
            }
        }

        public string StatusNameAR { get; set; }

        public string StatusNameEN { get; set; }
    }


}

