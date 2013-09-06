using System;
using System.Web.Mvc;
using dFrontierAppWizard.Core;
using dFrontierAppWizard.WebUI.Dto;

namespace dFrontierAppWizard.WebUI.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Index(Exception error)
        {
            Response.StatusCode = 500;
             var m = error.Message;
             if (error.InnerException != null) m += " | " + error.InnerException.Message;
                ViewBag.Message = m;
                if(Request.IsAjaxRequest())
            {
                if (error is AppWizardException)
                    return View("Expectedp");
                return View("Errorp");
            }

            if (error is AppWizardException)
                return View("Expected", new ErrorDisplay { Message = error.Message });
            return View("Error", new ErrorDisplay{Message = error.Message});
        }

        public ActionResult HttpError404(Exception error)
        {
            Response.StatusCode = 404;
            return View();
        }

        public ActionResult HttpError505(Exception error)
        {
            Response.StatusCode = 505;
            return View();
        }
    }
}