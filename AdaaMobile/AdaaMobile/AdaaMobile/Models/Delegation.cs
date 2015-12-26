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
    public class Delegation
    {
        private string managerNameField;

        private ushort managerIDField;

        private string delegateNameField;

        private uint delegateIDField;

        private string subordinateNameField;

        private ushort subordinateIDField;

        /// <remarks/>
        public string ManagerName
        {
            get
            {
                return this.managerNameField;
            }
            set
            {
                this.managerNameField = value;
            }
        }

        /// <remarks/>
        public ushort ManagerID
        {
            get
            {
                return this.managerIDField;
            }
            set
            {
                this.managerIDField = value;
            }
        }

        /// <remarks/>
        public string DelegateName
        {
            get
            {
                return this.delegateNameField;
            }
            set
            {
                this.delegateNameField = value;
            }
        }

        /// <remarks/>
        public uint DelegateID
        {
            get
            {
                return this.delegateIDField;
            }
            set
            {
                this.delegateIDField = value;
            }
        }

        /// <remarks/>
        public string SubordinateName
        {
            get
            {
                return this.subordinateNameField;
            }
            set
            {
                this.subordinateNameField = value;
            }
        }

        /// <remarks/>
        public ushort SubordinateID
        {
            get
            {
                return this.subordinateIDField;
            }
            set
            {
                this.subordinateIDField = value;
            }
        }
    }
}

