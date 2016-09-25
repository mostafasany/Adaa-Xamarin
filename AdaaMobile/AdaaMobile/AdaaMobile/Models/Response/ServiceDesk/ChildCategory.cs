using System.Collections.Generic;


namespace AdaaMobile.Models.Response.ServiceDesk
{
    public class ChildCategoryResult
    {
        public List<ChildCategory> result { get; set; }
        public object Exception { get; set; }
    }

    public class ChildCategory
    {
        public string id { get; set; }
        public string name { get; set; }
        public string parent { get; set; }
        public int order { get; set; }
    }
}
