using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace APITemplateCore.BusinessServices.Helpers
{
    public static class XmlExtensions
    {
        public static string SerializeXML<T>(this T source) where T : class, new()
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, source);
                return writer.ToString();
            }
        }

        public static T DeserializeXML<T>(this string XML) where T : class, new()
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var reader = new StringReader(XML))
            {
                return (T)serializer.Deserialize(reader);
            }
        }

        public static T DeserializeXmlFromFile<T>(string fileName)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var fs = new FileStream(fileName, FileMode.Open))
            {
                using (var reader = XmlReader.Create(fs))
                {
                    return (T)serializer.Deserialize(reader);
                }
            }
        }
    }
}
