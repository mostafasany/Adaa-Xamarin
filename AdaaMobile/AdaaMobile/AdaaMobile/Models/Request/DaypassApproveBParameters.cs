using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AdaaMobile.Models.Request
{
  public  class DaypassApproveBParameters
    {


        [XmlElement("comment")]
        public string comment { get; set; }


    }

}
