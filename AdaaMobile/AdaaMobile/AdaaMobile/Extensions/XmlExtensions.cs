using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AdaaMobile.Extensions
{
    public static class XmlExtensions
    {
        public static string SerializeXml<TR>(this TR obj) where TR : new()
        {
			XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
			ns.Add("","");

            var serializer = new XmlSerializer(typeof(TR));
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, obj,  ns);
				string xmlString = writer.ToString();

				if (xmlString.Contains (@"<?xml version=""1.0"" encoding=""utf-16""?>")) {
					xmlString = xmlString.Replace(@"<?xml version=""1.0"" encoding=""utf-16""?>","");
				}
				return xmlString;
            }
        }

        public static TR DeserializeXml<TR>(this string xml) where TR : new()
        {
            var serializer = new XmlSerializer(typeof(TR));
            using (var reader = new StringReader(xml))
            {
                return (TR)serializer.Deserialize(reader);
            }
        }
    }
}
