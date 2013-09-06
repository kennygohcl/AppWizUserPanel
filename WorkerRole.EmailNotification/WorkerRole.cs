using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using WorkerRole.EmailNotification.Base;
using WorkerRole.EmailNotification.Model;
using Microsoft.AppFabricCAT.Samples.Azure.TransientFaultHandling;
using Microsoft.AppFabricCAT.Samples.Azure.TransientFaultHandling.SqlAzure;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.Objects;

namespace WorkerRole.EmailNotification
{
    public class WorkerRole : RoleEntryPoint
    {
        public override void Run()
        {
            
            Trace.WriteLine("WorkerRole.EmailNotification entry point called", "Information");

            while (true)
            {
                // Clean Up The Unfinished Jobs
                CleanUp();

               

                DateTimeOffset nextExecutionTime = new DateTime(
                    DateTime.UtcNow.Year,
                    DateTime.UtcNow.Month,
                    DateTime.UtcNow.Day,
                    3, 0, 0);
                if (DateTimeOffset.UtcNow > nextExecutionTime)
                {
                   //if(LoadTrialExpirations())
                 //  {
                        using (Model.Entities ctx = new Model.Entities())
                        {
                            foreach (var job in ctx.vwJobsToExecutes)
                            {
                                //  Try to Get a Job Id.
                                Guid? jobId = StartJob(job);
                                if (jobId.HasValue)
                                {
                                    Trace.WriteLine("Working", "Information");
                                    // This Method Has the Code That Execute
                                    // A Stored Procedure, The Actual Job
                                    ExecuteJob(job);
                                    StopJob(jobId.Value);
                                }
                            }
                        }
                       SendTrialNotificationEmails();
                   //}

                    Thread.Sleep(new TimeSpan(0, 0, 10));
                }
                else
                {

                    Thread.Sleep(new TimeSpan(0, 0, 10));
                }
            }
  
        }

        private void SendTrialNotificationEmails()
        {
            using (Model.Entities ctx = new Model.Entities())
            {
                foreach (var sendEmail in ctx.sysmail_mailitems.Where(a=>a.sent_status==0))
                {
                   
                    try
                    {
                      
                        Trace.WriteLine("Email Working", "Information");
                        Thread.Sleep(ConfigSettings.EmailQueueInterval);

                        var selectedProfileAccount = ctx.sysmail_profileaccount.Where(a => a.profile_id == sendEmail.profile_id).SingleOrDefault();
                        var selectedSystemAccount =ctx.sysmail_account.Where(a => a.account_id == selectedProfileAccount.account_id).SingleOrDefault();
                        var selectedAccountCredential = ctx.sysmail_account_credential.Where(a => a.credential_id == selectedSystemAccount.account_id).SingleOrDefault();
                        var server = selectedSystemAccount.sysmail_server.SingleOrDefault();
                       
                        EmailManager.EmailAddress = selectedSystemAccount.email_address;
                        EmailManager.EmailDisplayName = selectedSystemAccount.display_name;
                        EmailManager.EmailHost = server.servername;
                        EmailManager.EmailPort = server.port;
                        EmailManager.EmailUser = selectedAccountCredential.username;
                        string encriptedPassword = Util.Encrypt("Pinkman1!x", "flumiwes");

                        EmailManager.EmailPassword = encriptedPassword;    //selectedAccountCredential.cyphertext;
                        EmailManager.QueueEmailMessage(selectedSystemAccount.email_address, selectedSystemAccount.display_name, sendEmail.recipients, "", "", "", sendEmail.subject, sendEmail.body, DateTime.Now, 1, true);
                      
                        EmailManager.SendQueuedEmail(sendEmail.mailitem_id, selectedSystemAccount.account_id);

                       
                      
                    }
                    catch (Exception e)
                    {
                        Trace.TraceError("Email Error: " + e.Message);
                    }

                }
            }
        }
         
       

        public override bool OnStart()
        {
          

            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            RoleEnvironment.Changing += RoleEnvironmentChanging;

            // This code sets up a handler to update CloudStorageAccount instances when their corresponding
            // configuration settings change in the service configuration file.
            CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) =>
            {
                // Provide the configSetter with the initial value
                configSetter(RoleEnvironment.GetConfigurationSettingValue(configName));

                RoleEnvironment.Changed += (sender, arg) =>
                {
                    if (arg.Changes.OfType<RoleEnvironmentConfigurationSettingChange>()
                        .Any((change) => (change.ConfigurationSettingName == configName)))
                    {
                        // The corresponding configuration setting has changed, propagate the value
                        if (!configSetter(RoleEnvironment.GetConfigurationSettingValue(configName)))
                        {
                            RoleEnvironment.RequestRecycle();
                        }
                    }
                };
            });

            return base.OnStart();
        }

        #region Scheduled Tasks
        //private bool LoadTrialExpirations()
        //{
        //    string connString =DefaultConnectionString();
        //    using (var con = new SqlConnection(connString))
        //    {
        //        try
        //        {
        //            con.Open();
        //            using (var cmd = con.CreateCommand())
        //            {
        //                cmd.CommandText = "spLoadExpiringTrials";
        //                cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //                cmd.ExecuteNonQuery();
        //            }
        //            con.Close();
        //            return true;
        //        }
        //        catch (SqlException)
        //        {
        //            Trace.TraceError("SqlException");
        //            throw;
        //        }
        //    }
        //    return false;
        //}

        #endregion


        #region JobMethods
        #region DefaultConnectionString
        private string DefaultConnectionString()
        {
            return ConfigurationManager
                .ConnectionStrings["DefaultSqlAzure"]
                .ConnectionString;
        }
        #endregion

        #region ExecuteJob
        private void ExecuteJob(vwJobsToExecute job)
        {
            string connString =
                (string.IsNullOrEmpty(job.ConnectionStringToUse))
                ? DefaultConnectionString()
                : job.ConnectionStringToUse;

            using (var con = new SqlConnection(connString))
            {
                try
                {
                    con.Open();
                    using (var cmd = con.CreateCommand())
                    {
                        cmd.CommandText = job.SqlToExecute;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (SqlException)
                {
                    Trace.TraceError("SqlException");
                    throw;
                }
            }
        }
        #endregion

        #region StartJob
        private Guid? StartJob(vwJobsToExecute job)
        {
            Guid? retValue = null;
            using (Entities ctx = new Entities())
            {
                var activityIdParam =
                    new ObjectParameter("ActivityId", typeof(Guid?));
                ctx.StartJob(job.JobId, activityIdParam);
                if (activityIdParam.Value != null
                    && activityIdParam.Value.GetType()
                    != typeof(System.DBNull))
                {
                    retValue = (Guid)activityIdParam.Value;
                }
            }
            return retValue;
        }
        #endregion

        #region StopJob
        private void StopJob(Guid activityId)
        {
            var rPolicy = new RetryPolicy<SqlAzureTransientErrorDetectionStrategy>
                (5, TimeSpan.FromMilliseconds(150));
            using (ReliableSqlConnection con =
                new ReliableSqlConnection(DefaultConnectionString(), rPolicy))
            {
                con.Open();
                using (var cmdStop = con.CreateCommand())
                {
                    cmdStop.CommandType = System.Data.CommandType.StoredProcedure;
                    cmdStop.CommandText = "StopJob";
                    cmdStop.Parameters.AddWithValue("@ActivityId", activityId);
                    con.ExecuteCommand(cmdStop, rPolicy);
                }
            }
        }
        #endregion

        #region CleanUp
        private void CleanUp()
        {
            using (Entities ctx = new Entities())
            {
                ctx.CleanUp();
            }
        }
        #endregion

        private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {
            // If a configuration setting is changing
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
            {
                // Set e.Cancel to true to restart this role instance
                e.Cancel = true;
            }
        }
        #endregion
  
    }
}
