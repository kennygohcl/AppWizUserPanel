using System;
using Microsoft.WindowsAzure.StorageClient;

namespace WorkerRole.EmailNotification.Base
{
   
    public class EmailMessageEntity : TableServiceEntity
    {
        public string From { get; set; }

        public string FromName { get; set; }

        public string To { get; set; }

        public string ToName { get; set; }

        public string Cc { get; set; }

        public string Bcc { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public DateTime CreatedOn { get; set; }

        public int SendTries { get; set; }

        public DateTime SentOn { get; set; }

        public bool SendFail { get; set; }
    }
}

