using System.Web.Mvc;
using dFrontierAppWizard.Core.Model;
using dFrontierAppWizard.Core.Service;
using dFrontierAppWizard.WebUI.Dto;
using dFrontierAppWizard.WebUI.Mappers;

namespace dFrontierAppWizard.WebUI.Controllers
{
    public class HomeController : BaseController
    {
        

        public HomeController()
        {
            
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Index()
        {

            return View();
        }

        public ActionResult TermsAndConditions()
        {

            return View();
        }


        

    }
}