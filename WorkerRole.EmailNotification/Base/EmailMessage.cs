using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using Microsoft.WindowsAzure;

namespace WorkerRole.EmailNotification.Base
{
    [Serializable]
    public class EmailMessage
    {
        public Guid Id { get; set; }

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
    }
}
