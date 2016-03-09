﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AdaaMobile.Models.Response
{
    /// <remarks/>
    [XmlRoot("root")]
    public class GetClientsResponse
    {

        [XmlElement("msg")]
        public string Message { get; set; }



        [XmlElement("Item")]
        public BaseItem[] item
        {
            get;
            set;
        }
    }

}
