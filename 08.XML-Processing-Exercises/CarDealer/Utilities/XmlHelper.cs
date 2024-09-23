using System.Text;
using System.Xml.Serialization;

namespace CarDealer.Utilities
{
    public class XmlHelper
    {
        public T Deserialize<T>(string inputXml, string rootName)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute(rootName);
            XmlSerializer serializer = new XmlSerializer(typeof(T), xmlRoot);

            using StringReader reader = new StringReader(inputXml);

            T deserializedDtos = (T)serializer.Deserialize(reader);

            return deserializedDtos;
        }

        //Serialize<ExportDto[]>(ExportDto[], rootName)
        //Serialize<ExportDto>(ExportDto, rootName)
        public string Serialize<T>(T obj, string rootName)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute(rootName);
            XmlSerializer serializer = new XmlSerializer(typeof(T), xmlRoot);

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            StringBuilder sb = new StringBuilder();
            using StringWriter writer = new StringWriter(sb);

            serializer.Serialize(writer, obj, namespaces);

            return sb.ToString().TrimEnd();
        }

        //Serizliza<ExportDto>(ExportDto[], rootName)
        public string Serialize<T>(T[] obj, string rootName)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute(rootName);
            XmlSerializer serializer = new XmlSerializer(typeof(T[]), xmlRoot);

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            StringBuilder sb = new StringBuilder();
            using StringWriter writer = new StringWriter(sb);

            serializer.Serialize(writer, obj, namespaces);

            return sb.ToString().TrimEnd();
        }


    }
}
