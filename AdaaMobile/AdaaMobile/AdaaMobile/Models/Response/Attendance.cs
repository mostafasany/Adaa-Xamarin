﻿using System.Xml.Serialization;
using System;
using AdaaMobile.ViewModels;

namespace AdaaMobile.Models.Response
{

    [System.Xml.Serialization.XmlRoot(ElementName = "root", Namespace = "", IsNullable = false)]
	public partial class Assignment
    {
        public string Id { get; set; }

        public string Title { get; set; }

		public string Year { get; set; }

		public DateTime StartDate { get; set; }

		public string FinishDate { get; set; }

		public string AssignmentStatusCode { get{
				if (Locator.Default.AppSettings.SelectedCultureName.Contains ("ar")) {
					return AssignmentStatusNameAR;
				} else {
					return AssignmentStatusNameEN;
				}
			} }

		public string AssignmentStatusNameAR { get; set; }

		public string AssignmentStatusNameEN { get; set; }



    }


}

