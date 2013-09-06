using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.WindowsAzure.StorageClient;

namespace WorkerRole.EmailNotification.Base
{
    public static class ExtensionHelper
    {
      
        public static byte[] SerializeObject(object _object)
        {
            var memoryStream = new MemoryStream();
            var bf = new BinaryFormatter();
            bf.Serialize(memoryStream, _object);
            return memoryStream.ToArray();
        }

        public static T DeserializeObject<T>(byte[] bytes)
        {
            var memoryStream = new MemoryStream(bytes);
            var bf = new BinaryFormatter();
            return (T)bf.Deserialize(memoryStream);
        }

        public static byte[] Serialize<T>(this T model)
        {
            return SerializeObject(model);
        }

        public static T Deserialize<T>(this CloudQueueMessage cloudQueueMessage)
        {
            return DeserializeObject<T>(cloudQueueMessage.AsBytes);
        }
    }
}
