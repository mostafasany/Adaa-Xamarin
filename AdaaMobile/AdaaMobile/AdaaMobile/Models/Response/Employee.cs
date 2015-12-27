using System.Xml.Serialization;

namespace AdaaMobile.Models.Response
{

    public class Employee
    {

        [XmlElement("UserName")]
        public string UserName { get; set; }

        [XmlElement("UserID")]
        public string UserId { get; set; }


        public string NameSort
        {
            get
            {
				if (string.IsNullOrWhiteSpace(UserName) || UserName.Length == 0) return "A"; return UserName[0].ToString().ToUpper();
            }
        }
    }
}
