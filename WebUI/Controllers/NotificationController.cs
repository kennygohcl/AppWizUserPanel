using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using dFrontierAppWizard.Core.Model;
using dFrontierAppWizard.Core.Service;
using dFrontierAppWizard.WebUI.Dto;
using dFrontierAppWizard.WebUI.Mappers;

namespace dFrontierAppWizard.WebUI.Controllers
{
    public class NotificationController : Cruder<Notification, NotificationInput>
    {
        public NotificationController(ICrudService<Notification> service, IMapper<Notification, NotificationInput> v)
            : base(service, v)
        {

        }

        protected override string RowViewName
        {
            get { return "ListItems/Application"; }
        }
        
        [HttpPost]
        public string GetNotifications(int appId, int branchId)
        {
            var query = from selectedNotifications in service.Where(o => o.ApplicationId == appId && o.BranchId==branchId && o.IsDeleted == false).AsEnumerable()
                        select new
                        {
                            selectedNotifications.Message,
                            selectedNotifications.MessageTitle,
                            selectedNotifications.DateCreated,
                            selectedNotifications.BranchId,
                            selectedNotifications.ApplicationId
                        };

            var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string sJson = oSerializer.Serialize(query);
            sJson = @"{""Data"":" + sJson + @",""success"":true}";
            return sJson;
        }

    }
}


