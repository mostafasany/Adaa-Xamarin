using System;
using System.Xml.Serialization;
using AdaaMobile.Models;

namespace AdaaMobile
{

	[XmlRoot("root")]
	public class GetAllEmployeesResponse
	{

		private Employee[] itemField;

		   [XmlElement("Item")]
			public Employee[] Employees
			{
				get
				{
					return this.itemField;
				}
				set
				{
					this.itemField = value;
				}
			}
		}






}

