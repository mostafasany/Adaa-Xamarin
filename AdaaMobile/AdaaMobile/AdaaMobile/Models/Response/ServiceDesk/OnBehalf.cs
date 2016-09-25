using System.Collections.Generic;


namespace AdaaMobile.Models.Response.ServiceDesk
{
    public class OnBehalfResult
    {
        public List<OnBehalf> result { get; set; }
        public string Exception { get; set; }
    }

    public class OnBehalf
    {
        public string DisplayName { get; set; }
        public string userDomian { get; set; }
    }

}
