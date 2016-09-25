using System.Collections.Generic;

namespace AdaaMobile.Models.Response.ServiceDesk
{
    public class TemplateExtensionResult
    {
        public List<TemplateExtension> Table1 { get; set; }
    }

    public class TemplateExtension
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Type { get; set; }
        public object DefaultValue { get; set; }
        public string MaxLength { get; set; }
        public string EnumType { get; set; }
        public object MixedValue { get; set; }
        public bool Required { get; set; }
        public string EnumTypeId { get; set; }
        public string EnumListName { get; set; }
        public int ColumnOrder { get; set; }
    }

}
