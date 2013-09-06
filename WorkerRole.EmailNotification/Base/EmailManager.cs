using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using WorkerRole.EmailNotification.Model;


namespace WorkerRole.EmailNotification.Base
{
  
    public class EmailManager
    {
     // Manage account information
       private static CloudStorageAccount _storageAccount = null;

       // Cloud queue
       private static CloudQueue _cloudQueue = null;

       // Cloud client
       private static CloudQueueClient _queueClient = null;

        static EmailQueueClient GetEmailQueueClient()
        {
            LoadConfigSettings();

            return new EmailQueueClient(_storageAccount.QueueEndpoint.AbsoluteUri, _storageAccount.Credentials, "email-queue");
            
        }



        //static BaseServiceContext<EmailMessageEntity> GetEmailMessageContext()
        //{
           
        //    return new BaseServiceContext<EmailMessageEntity>(_storageAccount.TableEndpoint.AbsoluteUri, _storageAccount.Credentials);
        //}

        #region Email Template

        private const string EmailTemplate = @"
                        <!DOCTYPE HTML PUBLIC '-//W3C//DTD HTML 4.0 Transitional//EN'>
                        <html>
	                        <head>
		                        <title></title>
                                <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
                                <meta http-equiv='Content-Language' content='en-us' />
                                <style type='text/css' media='screen'>
                                    table { font-size:14px; font-family: arial; color:#434244;}
                                    a {text-decoration:none;}
                                    a {color:#006C40;}
                                    a:hover { color:#006C40;text-decoration: underline;}
                                </style>
	                        </head>
	                        <body>	    
                                <table>        
                                <tr><td></td>
       
                                <td>
                                <#CONTENT#>
                                <br/><br/>        
                                Thank you for using AppWiz,<br/>
                                AppWiz Team
                                </td>
       
                                <td></td></tr>
                                </table>
	                        </body>
                        </html>";

        #endregion Email Template

        #region Properties

        public static string EmailAddress {get; set;}

        public static string EmailDisplayName{get; set;}

        public static string EmailHost{get; set;}

        public static int EmailPort{get;set;}

        public static string EmailUser{get;set;}

        public static string EmailPassword{get;set;}

        public static bool EmailUseDefaultCredentials
        {
            get { return ConfigSettings.EmailUseDefaultCredentials; }
        }

        public static bool EmailEnableSsl
        {
            get
            {
                return ConfigSettings.EmailEnableSsl;
            }
        }

        public static int EmailQueueInterval
        {
            get
            {
                return ConfigSettings.EmailQueueInterval;
            }
        }
        #endregion

        #region Private Methods

        private static void LoadConfigSettings()
        {
            _storageAccount = CloudStorageAccount.FromConfigurationSetting("Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString");
            
            _queueClient = _storageAccount.CreateCloudQueueClient();

            // Get and create the container
            _cloudQueue = _queueClient.GetQueueReference("email-queue");
            
            _cloudQueue.CreateIfNotExist();


        }

        //This will be used if Table storage is preferred
        //private static void PersistSentEmailMessage(Guid id, string from, string fromName, string to, string toName, string cc, string bcc, string subject, string body, DateTime createdOn, int sendTries, DateTime sentOn, bool success = true)
        //{
        //    var context = GetEmailMessageContext();
        //    var message = new EmailMessageEntity
        //    {
        //        Bcc = bcc,
        //        Body = body,
        //        Cc = cc,
        //        CreatedOn = createdOn,
        //        From = from,
        //        FromName = fromName,
        //        SendFail = !success,
        //        SentOn = sentOn,
        //        SendTries = sendTries,
        //        Subject = subject,
        //        To = to,
        //        ToName = toName,
        //        PartitionKey = DateTime.Now.Year + DateTime.UtcNow.DayOfYear.ToString("000"),
        //        RowKey = id.ToString(),
        //    };

        //    context.Add(message);
        //    context.SaveChanges();
        //}

        private static void SendEmail(string subject, string body, MailAddress from, MailAddress to, IEnumerable<string> bcc, IEnumerable<string> cc)
        {
            var message = new MailMessage { From = @from };
            message.To.Add(to);
            message.ReplyToList.Add(from);
            if (null != bcc)
            {
                foreach (string address in bcc)
                {
                    if (address != null)
                    {
                        if (!String.IsNullOrEmpty(address.Trim()))
                        {
                            message.Bcc.Add(address.Trim());
                        }
                    }
                }
            }

            if (null != cc)
            {
                foreach (string address in cc)
                {
                    if (address != null)
                    {
                        if (!String.IsNullOrEmpty(address.Trim()))
                        {
                            message.CC.Add(address.Trim());
                        }
                    }
                }
            }

            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient
            {
                UseDefaultCredentials = EmailUseDefaultCredentials,
                Host = EmailHost,
                Port = EmailPort,
                EnableSsl = EmailEnableSsl
            };
            
            string decriptedPassword = Util.Decrypt(EmailPassword, EmailUser);
            smtpClient.Credentials = EmailUseDefaultCredentials ? CredentialCache.DefaultNetworkCredentials : new NetworkCredential(EmailUser, decriptedPassword);

            smtpClient.Send(message);
        }
        #endregion

        #region Public Methods
        public static void QueueEmailMessage(MailAddress from, MailAddress to, string cc, string bcc, string subject, string body, DateTime createdOn, int sendTries, bool useStandardTemplate = true)
        {
            QueueEmailMessage(from.Address, from.DisplayName, to.Address, to.DisplayName, cc, bcc, subject, body, createdOn, sendTries, useStandardTemplate);
        }

        public static void QueueEmailMessage(string from, string fromName, string to, string toName, string cc, string bcc, string subject, string body, DateTime createdOn, int sendTries, bool useStandardTemplate)
        {
            var client = GetEmailQueueClient();

            if (useStandardTemplate)
                body = EmailTemplate.Replace("<#CONTENT#>", body);

            var emailMessage = new EmailMessage
            {
                From = from,
                FromName = fromName,
                To = to,
                ToName = toName,
                Cc = cc,
                Bcc = bcc,
                Subject = subject,
                Body = body,
                CreatedOn = createdOn,
                SendTries = sendTries,
                Id = Guid.NewGuid(),
            };

            client.AddMessageToQueue(new CloudQueueMessage(emailMessage.Serialize()));
        }

        public static void SendQueuedEmail(int mailItemId, int sentAccountId)
        {
            try
            {
               
                var queue = GetEmailQueueClient();
               
                

                int numberOfQueueMessage = ConfigSettings.EmailNumberOfQueueMessage;
                var cloudQueueMessages = queue.GetQueuedMessages(numberOfQueueMessage);


                if (cloudQueueMessages != null)
                {

                    foreach (var cloudQueueMessage in cloudQueueMessages)
                    {
                       
                        //queue.DeleteMessage(cloudQueueMessage);
                        var emailMessage = cloudQueueMessage.Deserialize<EmailMessage>();
                        if (emailMessage != null)
                        {
                            try
                            {
                                SendEmail(
                                    emailMessage.Subject,
                                    emailMessage.Body,
                                    new MailAddress(emailMessage.From, emailMessage.FromName),
                                    new MailAddress(emailMessage.To, emailMessage.ToName),
                                    new List<string> { emailMessage.Bcc },
                                    new List<string> { emailMessage.Cc });
                                //PersistSentEmailMessage(emailMessage.Id, emailMessage.From, emailMessage.FromName, emailMessage.To, emailMessage.ToName, emailMessage.Cc, emailMessage.Bcc, emailMessage.Subject, emailMessage.Body, emailMessage.CreatedOn, emailMessage.SendTries + 1, DateTime.UtcNow);
                                UpdateSysMailItem(mailItemId, sentAccountId, emailMessage.Body, 1);
                                queue.DeleteMessage(cloudQueueMessage);
                            }
                            catch (Exception e)
                            {
                                Trace.TraceError("Email Send Error: " + e.Message);

                                emailMessage.SendTries = emailMessage.SendTries + 1;
                                queue.DeleteMessage(cloudQueueMessage);

                                if (emailMessage.SendTries > 3)
                                {
                                    //PersistSentEmailMessage(emailMessage.Id, emailMessage.From,
                                    //                        emailMessage.FromName, emailMessage.To, emailMessage.ToName,
                                    //                        emailMessage.Cc, emailMessage.Bcc, emailMessage.Subject,
                                    //                        emailMessage.Body, emailMessage.CreatedOn, emailMessage.SendTries, DateTime.UtcNow, false);

                                    UpdateSysMailItem(mailItemId, sentAccountId, emailMessage.Body, 0);
                                }
                                else
                                {
                                    queue.AddMessageToQueue(new CloudQueueMessage(emailMessage.Serialize()));
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Trace.TraceError("Email Send Error: " + e.Message);
            }
        }

        private static void UpdateSysMailItem(int mailItemId, int sentAccountId, string emailBody, byte status)
        {
            using (Model.Entities ctx = new Model.Entities())
            {
                sysmail_mailitems toUpdateEmail = ctx.sysmail_mailitems.First(a => a.mailitem_id == mailItemId);
                toUpdateEmail.sent_status = status;
                toUpdateEmail.sent_date = DateTime.Now;
                toUpdateEmail.sent_account_id = sentAccountId;
                ctx.SaveChanges();
            }
        }

        public static void SendSimpleMessage(string emailTo, string recipientDisplayName, string subject, string text)
        {
            try
            {
                var from = new MailAddress(EmailAddress, EmailDisplayName);
                var to = new MailAddress(emailTo, recipientDisplayName);
                QueueEmailMessage(from, to, string.Empty, string.Empty, subject, text, DateTime.UtcNow, 0);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
            }
        }

        #endregion
    }
}
