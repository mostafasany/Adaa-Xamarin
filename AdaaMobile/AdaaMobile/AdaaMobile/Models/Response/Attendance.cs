﻿using System.Xml.Serialization;

namespace AdaaMobile.Models.Response
{

    [System.Xml.Serialization.XmlRoot(ElementName = "root", Namespace = "", IsNullable = false)]
    public partial class Attendance
    {
        [XmlElement("FirstSeen")]
        public string FirstSeen { get; set; }

        [XmlElement("LastSeen")]
        public string LastSeen { get; set; }

        [XmlElement("Duration")]
        public string Duration { get; set; }
    }


}

