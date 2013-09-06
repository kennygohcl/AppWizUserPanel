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
    public class PaymentGatewayController : Cruder<PaymentGateway, PaymentGatewayInput>
    {
        private readonly IUserService _us;

        public PaymentGatewayController(ICrudService<PaymentGateway> service, IMapper<PaymentGateway, PaymentGatewayInput> v, IUserService us)
            : base(service, v)
        {
            this._us = us;
        }

        public PaymentGatewayInput paymentGatewayInput = new PaymentGatewayInput();

        public override ActionResult Create()
        {
            ViewBag.UserId = _us.GetUserIdByName(HttpContext.User.Identity.Name);
            return View(paymentGatewayInput);
        }

        protected override string RowViewName
        {
            get { return "ListItems/PaymentGateway"; }
        }
    }
}
