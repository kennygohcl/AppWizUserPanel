using System.Collections.Generic;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace WorkerRole.EmailNotification.Base
{
    public class EmailQueueClient : CloudQueueClient
    {
        private readonly string _queueName = "";
        public EmailQueueClient(string baseAddress, StorageCredentials credentials, string queueName)
            : base(baseAddress, credentials)
        {
            _queueName = queueName;
        }

        public CloudQueueMessage GetQueuedMessage()
        {
            return GetQueueReference(_queueName).GetMessage();
        }

        public IEnumerable<CloudQueueMessage> GetQueuedMessages(int count)
        {
            return GetQueueReference(_queueName).GetMessages(count);
        }

        public void AddMessageToQueue(CloudQueueMessage message)
        {
            var queue = GetQueueReference(_queueName);
            queue.CreateIfNotExist();
            queue.AddMessage(message);
        }

        public void DeleteMessage(CloudQueueMessage cloudQueueMessage)
        {
            GetQueueReference(_queueName).DeleteMessage(cloudQueueMessage);
        }
    }
}
