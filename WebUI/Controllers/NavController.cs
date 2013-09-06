using System.Web.Mvc;
using dFrontierAppWizard.Core.Service;

namespace dFrontierAppWizard.WebUI.Controllers
{
    public class NavController : Controller
    {

         private new readonly IUserService userService;
         private new readonly IUserBillingInformationService service;
         public NavController(IUserService userService, IUserBillingInformationService service)
        {
            this.userService = userService;
             this.service = service;
        }

        public ActionResult Index()
        {
            int userTypeId = userService.GetUserTypeId(HttpContext.User.Identity.Name);
            int userId = userService.GetUserIdByName(HttpContext.User.Identity.Name);
            bool hasBillingInfo = service.HasBillingInfo(userId);
            ViewBag.UserTypeId = userTypeId;
            ViewBag.HasBillingInfo = hasBillingInfo; 
            return View();
        }
    }
}