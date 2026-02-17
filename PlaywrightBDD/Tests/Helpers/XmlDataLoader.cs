using System.IO;
using System.Xml.Serialization;

namespace Tests.Helpers
{
    public static class XmlDataLoader
    {
        public static T Load<T>(string fileName)
        {
            var path = Path.Combine("DATA", fileName);

            if (!File.Exists(path))
                throw new FileNotFoundException($"XML file not found: {path}");

            using var stream = new FileStream(path, FileMode.Open);
            var serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(stream)!;
        }
    }
}