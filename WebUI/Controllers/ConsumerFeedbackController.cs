
using System;
using System.Linq;
using System.Web.Mvc;
using dFrontierAppWizard.Core.Model;
using dFrontierAppWizard.Core.Repository;
using dFrontierAppWizard.Core.Service;
using dFrontierAppWizard.WebUI.Dto;
using dFrontierAppWizard.WebUI.Mappers;

namespace dFrontierAppWizard.WebUI.Controllers
{
    public class ConsumerFeedbackController : Cruder<ConsumerFeedback, ConsumerFeedbackInput>
    {
        private readonly IUserService _us;
        private new readonly IRepo<Lead> leadService;
        public ConsumerFeedbackController(ICrudService<ConsumerFeedback> service, IMapper<ConsumerFeedback, ConsumerFeedbackInput> v, IUserService us, IRepo<Lead> leadService)
            : base(service, v)
        {
            this._us = us;
            this.leadService = leadService;
        }

        protected override string RowViewName
        {
            get { return "ListItems/ConsumerFeedback"; }
        }

      

        [HttpPost]
        public ActionResult SetConsumerFeedback(int consumerId, string comment, string status)
        {

            if (consumerId > 0 && comment.Length > 0)
            {
                var consumerFeedback = service.Create(new ConsumerFeedback
                {
                    ConsumerId = consumerId,
                    Comments = comment,
                    Status=status
                });

                service.Save();
                return Json(true);
            }

           /* var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string sJson = oSerializer.Serialize(query);
            sJson = @"{""Data"":" + sJson + @",""success"":true}";
            return sJson;
*/
            return Json(false);
        }

        [HttpPost]
        public ActionResult SetRefer(int consumerId, string name, string email, string mobile, string referredBy)
        {

            if (email.Length > 0 && name.Length > 0)
            {
                var lead = leadService.Insert(new Lead
                {
                    FirstName = name,
                    LastName = "XXXXXX",
                    EmailAddress = email,
                    ReferredBy = consumerId.ToString() +"|"+ referredBy,
                    Mobile = mobile,
                    Address = "XXXXXX",
                    DateCreated = DateTime.Now
                });
                leadService.Save();


                return Json(true);
            }

            /* var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
             string sJson = oSerializer.Serialize(query);
             sJson = @"{""Data"":" + sJson + @",""success"":true}";
             return sJson;
 */
            return Json(false);
        }

    }

}