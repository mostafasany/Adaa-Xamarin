﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AdaaMobile.Models.Response
{
    public class delegationSubordinatesResponse
    {
        [XmlElement("msg")]
        public string Message { get; set; }


        [XmlElement("Item")]
        public Employee[] Subordinates { get; set; }

    }
}
