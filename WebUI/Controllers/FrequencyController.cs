using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using NPOI.HSSF.UserModel;
using dFrontierAppWizard.Core.Model;
using dFrontierAppWizard.Core.Repository;
using dFrontierAppWizard.Core.Service;
using dFrontierAppWizard.WebUI.Dto;
using dFrontierAppWizard.WebUI.Mappers;
using dFrontierAppWizard.Data;
//using Microsoft.Office.Interop.Excel;
using dFrontierAppWizard.WebUI.Utils;

namespace dFrontierAppWizard.WebUI.Controllers
{

    public class FrequencyController : Cruder<Frequency, FrequencyInput>
    {
        private new readonly IUserService userService;
        private new readonly IApplicationService applicationService;


        public FrequencyController(ICrudService<Frequency> service, IMapper<Frequency, FrequencyInput> v, IUserService userService, IApplicationService applicationService)
            : base(service, v)
        {
            this.userService = userService;
            this.applicationService = applicationService;
        }


        [HttpPost]
        public ActionResult SetFrequency(int hiddenIdValue, string txtVoucherMessageFrequency)
        {
         
            var freq = service.Create(new Frequency
            {
                ApplicationId = hiddenIdValue,
                Description = txtVoucherMessageFrequency
            });
            service.Save();


         
            return Json(new { status = "true" });

        }

        protected override string RowViewName
        {
            get { return "ListItems/Frequency"; }
        }

        public override ActionResult Index()
        {
            int id = userService.GetUserIdByName(HttpContext.User.Identity.Name);
            var app = applicationService.Where(o => o.UserId.Equals(id)).SingleOrDefault();
            if (app != null)
            {
                Session["appId"] = app.Id;
                Session["appName"] = app.ApplicationName;
            }

            if (Session["appId"] != null)
            {
                ViewBag.ApplicationId = Session["appId"];

            }
            return View();
        }
        
        public ActionResult IndexNoHeader()
        {
            if (Session["appId"]!=null)
            {
                ViewBag.ApplicationId = Session["appId"];

            }
            return View();
        }
    }
}
