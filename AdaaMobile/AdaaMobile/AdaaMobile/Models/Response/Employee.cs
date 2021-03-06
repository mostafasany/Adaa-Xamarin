﻿using System.Xml.Serialization;
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

		[XmlElement("DeptName")]
		public string DeptName { get; set; }

		[XmlElement("GroupName")]
		public string GroupName { get; set; }

		[XmlElement("OfficeNum")]
		public string OfficeNum { get; set; }

		[XmlElement("MobileNum")]
		public string MobileNum { get; set; }

		public string NameSort
		{
			get
			{
				if (string.IsNullOrWhiteSpace(UserName) || UserName.Length == 0) return "A"; return UserName[0].ToString().ToUpper();
			}
		}
	}
}
