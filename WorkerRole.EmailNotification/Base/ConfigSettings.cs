using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace WorkerRole.EmailNotification.Base
{
    public class ConfigSettings
    {
        public static readonly int EmailQueueInterval = Int32.Parse(ConfigurationManager.AppSettings["Email.QueueInterval"]);
        public static readonly bool EmailEnableSsl = bool.Parse(ConfigurationManager.AppSettings["Email.EnableSsl"]);
        public static readonly bool EmailUseDefaultCredentials = bool.Parse(ConfigurationManager.AppSettings["Email.UseDefaultCredentials"]);
        public static readonly int EmailNumberOfQueueMessage = int.Parse(ConfigurationManager.AppSettings["Email.NumberOfQueueMessage"]);
    }
}
