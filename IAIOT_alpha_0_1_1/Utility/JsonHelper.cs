using Newtonsoft.Json;
using System.IO;

namespace Final_project_IAIOTCloud.Utility
{
    public class JsonHelper
    {
        public static T Deserialize<T>(string content) where T : class, new()
        {
            return JsonConvert.DeserializeObject<T>(content);
        }

        public static string Serialize<T>(T obj) where T : class, new()
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static void Serialize<T, S>(T obj, S stream) where S : Stream where T : class, new()
        {
            using (stream)
            {
                byte[] content = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj));
                stream.Write(content, 0, content.Length);
            }
        }
    }
}
