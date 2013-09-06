//------------------------------------------------------------------------------
// <copyright file="WebDataService.svc.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data.Services;
using System.Data.Services.Common;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web;
using System.Web.Script.Serialization;
using Omu.Encrypto;

using dFrontierAppWizard.Core.Model;

namespace WcfService
{
    public class AppWizWcfDataService : DataService<dbDFrontierAppWizardEntities>
    {
       

 

        // This method is called only once to initialize service-wide policies.
        public static void InitializeService(DataServiceConfiguration config)
        {
            // TODO: set rules to indicate which entity sets and service operations are visible, updatable, etc.
            // Examples:
           //  config.SetEntitySetAccessRule("*", EntitySetRights.AllRead);
            // config.SetServiceOperationAccessRule("MyServiceOperation", ServiceOperationRights.All);
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V3;
            config.SetServiceOperationAccessRule("GetAuthenticatedUser", ServiceOperationRights.All);
         
        }

        //[WebGet(UriTemplate = "{text}")]
        //public IQueryable<string> SplitString(string text)
        //{
        //    if (text == null) throw new DataServiceException("text not specified");
        //    var result = (from s in text.Split('-') orderby s select s);
        //    return result.AsQueryable();
        //}


       // [WebGet(UriTemplate = "/GetAuthenticatedUser/{loginName}/{password}", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]



        [WebGet(UriTemplate = "GetAuthenticatedUser?loginName={loginName}&password={password}", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        public string GetAuthenticatedUser(string loginName, string password)
        {
            var hasher = new Hasher();
            hasher.SaltSize = 10;
            dbDFrontierAppWizardEntities db = new dbDFrontierAppWizardEntities();
            var selectedUser =db.Users.Where(o => o.Login == loginName && o.IsDeleted == false).SingleOrDefault();
            if (selectedUser == null || !hasher.CompareStringToHash(password, selectedUser.Password))
            {
                return null;
            }
  
            string userGuid = null;

            userGuid = selectedUser.Login + "|" + System.Guid.NewGuid();
            return new JavaScriptSerializer().Serialize(userGuid);
           
        }

      
      

    }

  

}
