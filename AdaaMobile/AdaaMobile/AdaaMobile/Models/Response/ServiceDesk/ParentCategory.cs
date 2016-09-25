using System.Collections.Generic;


namespace AdaaMobile.Models.Response.ServiceDesk
{
    public class ParentCategoryResult
    {
        public List<ParentCategory> result { get; set; }
        public object Exception { get; set; }
    }

    public class ParentCategory
    {
        public string id { get; set; }
        public string name { get; set; }
        public string parent { get; set; }
        public int order { get; set; }
    }
}


