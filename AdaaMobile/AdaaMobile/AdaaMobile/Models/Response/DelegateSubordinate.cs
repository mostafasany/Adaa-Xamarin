using System.Xml.Serialization;

namespace AdaaMobile.Models.Response
{
    /// <remarks/>
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class DelegateSubordinate
    {
        [XmlElement("UserName")]
        public string UserName { get; set; }

        [XmlElement("UserID")]
        public ushort UserId { get; set; }


        public string NameSort
        {
            get
            {
                if (string.IsNullOrWhiteSpace(UserName) || UserName.Length == 0) return "?"; return UserName[0].ToString().ToUpper();
            }
        }

    }

}

