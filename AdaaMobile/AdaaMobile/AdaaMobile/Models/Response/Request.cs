using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Xamarin.Forms;

namespace AdaaMobile.Models.Response
{
    public class Request
    {
        [XmlElement("title")]
        public string title
        {
            get;
            set;
        }

        [XmlElement("service")]
        public string service
        {
            get;
            set;
        }

        [XmlElement("created")]
        public string created
        {
            get;
            set;
        }

        [XmlElement("assigned")]
        public string assigned
        {
            get;
            set;
        }

        [XmlElement("status")]
        public string status
        {
            get;
            set;
        }

	
		public Color statusColor
		{
			get {
				if (status == "In Progress" || status=="قيد الإنشاء") {
					return Color.Gray;
				}
				else if (status == "Approved" || status=="موافق عليها") {
					return Color.Green;
				}
				else {
					return Color.Red;
				}

			}

		}

        [XmlElement("url")]
        public string url
        {
            get;
            set;
        }
    }
}
