using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaaMobile.Models
{
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public class DelegateSubordinate
    {

        private string userNameField;

        private ushort userIDField;

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
        public ushort UserID
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
                if (string.IsNullOrWhiteSpace(UserName) || UserName.Length == 0) return "?"; return UserName[0].ToString().ToUpper();
            }
        }

    }

}

