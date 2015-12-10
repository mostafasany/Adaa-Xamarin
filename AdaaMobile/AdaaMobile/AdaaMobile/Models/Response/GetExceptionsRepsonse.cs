using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AdaaMobile.Models.Response
{
    //TODO:Test when implemented in Backend.
    [System.Xml.Serialization.XmlRoot(ElementName = "root", Namespace = "", IsNullable = false)]
    public class GetExceptionsRepsonse
    {
        [XmlElement("msg")]
        public string Message { get; set; }

        [XmlElement("Item")]
        public ExceptionDay[] ExceptionDays { get; set; }
    }

    public class ExceptionDay
    {
        [XmlElement("RawDate")]
        public string RawDate { get; set; }

        public DateTime Date { get; set; }
    }
}
