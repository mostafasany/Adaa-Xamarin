using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaaMobile.Models
{
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class Employee
    {
        public string Name { get; set; }
        private string userNameField;

        private uint userIDField;

        /// <remarks/>
        public string UserName
        {
            get
            {
                return this.userNameField;
            }
            set
            {
                this.userNameField = value;
            }
        }

        /// <remarks/>
        public uint UserID
        {
            get
            {
                return this.userIDField;
            }
            set
            {
                this.userIDField = value;
            }
        }


        public string NameSort
        {
            get
            {
				if (string.IsNullOrWhiteSpace(UserName) || UserName.Length == 0) return "A"; return UserName[0].ToString().ToUpper();
            }
        }
    }
}
