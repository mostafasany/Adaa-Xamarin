﻿using System.Xml.Serialization;

namespace AdaaMobile.Models.Response
{
    [XmlRoot("root", IsNullable = true)]
    public class DayPassTasksResponse
    {
        [XmlElement("msg")]
        public string Message { get; set; }

        [XmlElement("Item")]
        public DayPassTask[] DayPassTaskList { get; set; }
    }
}

