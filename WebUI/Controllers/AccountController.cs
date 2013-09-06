using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using dFrontierAppWizard.Core.Model;
using dFrontierAppWizard.Core.Security;
using dFrontierAppWizard.Core.Service;
using dFrontierAppWizard.Resources;
using dFrontierAppWizard.WebUI.Dto;

namespace dFrontierAppWizard.WebUI.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IFormsAuthentication formsAuth;
        private readonly IUserService us;
        private readonly IUserBillingInformationService bi;

        public AccountController(IFormsAuthentication formsAuth, IUserService us, IUserBillingInformationService bi)
        {
            this.formsAuth = formsAuth;
            this.us = us;
            this.bi = bi;
        }

        public ActionResult SignIn()
        {
            return View();
        }

        public ActionResult JoinNow()
        {
            return View();
        }

       

        [HttpPost]
        public ActionResult SignIn(SignInInput input)
        { 
            if (!ModelState.IsValid)
            {
                input.Password = null;
                input.Login = null;
                return View(input);
            }

            var user = us.Get(input.Login, input.Password);
   
            if (user == null)
            {
                ModelState.AddModelError("", "Try Login: o and Password: 1");
               // ModelState.AddModelError(Mui.Security, Mui.Invalid_Username_or_Password);
                return View();
            }

            Session["userId"] = user.Id;
            formsAuth.SignIn(user.Login, input.Remember, user.Roles.Select(o => o.Name));
            var list = bi.Where(o => o.UserId.Equals(user.Id)).SingleOrDefault();
            if (list==null)
            {
                if (user.UserTypeId==2)
                {
                    return RedirectToAction("CreateBilling", "UserBillingInformation");
                }
                else
                {
                    return RedirectToAction("index", "Application"); 
                }
               
            }
            else
            {
                if (user.UserTypeId == 1)
                {
                    return RedirectToAction("index", "Home");
                }
                else if (user.UserTypeId == 2)
                {
                    return RedirectToAction("index", "Application");
                }
                else
                {
                    return RedirectToAction("index", "Home");
                }
            }
            
        }

        public ActionResult SignOff()
        {
            Session["userId"] = 0;
            formsAuth.SignOut();
            return RedirectToAction("SignIn", "Account");
        }
    }
}