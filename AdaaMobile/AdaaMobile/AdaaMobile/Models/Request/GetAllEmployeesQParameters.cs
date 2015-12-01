using System;
using QueryExtensions;

namespace AdaaMobile
{
	public class GetAllEmployeesQParameters
	{
		
		[QueryParameter ("server")]
		public string Server { get; set; }

		[QueryParameter ("url")]
		public string Url { get; set; }

		[QueryParameter ("langid")]
		public string Langid { get; set; }

		[QueryParameter ("userToken")]
		public string UserToken { get; set; }

	}
		

}

