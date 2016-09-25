using System.Collections.Generic;

namespace AdaaMobile.Models.Response.ServiceDesk
{
    public class CategoryTemplateResult
    {
        public List<CategoryTemplate> result { get; set; }
        public object Exception { get; set; }
    }

    public class CategoryTemplate
    {
        public string ID { get; set; }
        public string CategoryPath { get; set; }
        public string TemplateName { get; set; }
        public string CategoryID { get; set; }
        public string TemplateID { get; set; }
        public string ClassName { get; set; }
    }

}
