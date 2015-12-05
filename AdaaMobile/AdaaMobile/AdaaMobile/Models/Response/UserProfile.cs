using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AdaaMobile.Models.Response
{
 
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [XmlRoot("root", Namespace = "", IsNullable = false)]
        public partial class UserProfile
        {
            [XmlElement("DisplayName")]
            public string DisplayName { get; set; }

            [XmlElement("UserID")]
            public string UserId { get; set; }

            [XmlElement("JobTitle")]
            public string JobTitle { get; set; }

            [XmlElement("DeptName")]
            public string DeptName { get; set; }

            [XmlElement("GroupName")]
            public string GroupName { get; set; }

            [XmlElement("OfficeNum")]
            public string OfficeNum { get; set; }

            [XmlElement("MobileNum")]
            public string MobileNum { get; set; }

            [XmlElement("Email")]
            public string Email { get; set; }

            [XmlElement("Manager")]
            public string Manager { get; set; }

            [XmlElement("UserImage64")]
            public string UserImage64 { get; set; }
        }
    
}
