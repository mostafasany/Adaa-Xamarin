using System.Xml.Serialization;
using AdaaMobile.Common;

namespace AdaaMobile.Models.Response
{

	public class Employee:BindableBase
    {

		private bool _isSelected;
		public bool IsSelected
		{
			get { return _isSelected; }
			set { SetProperty(ref _isSelected, value); }
		}


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
