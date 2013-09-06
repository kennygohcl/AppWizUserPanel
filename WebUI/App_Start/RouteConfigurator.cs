using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.ComponentModel.Composition;

namespace dFrontierAppWizard.WebUI.App_Start
{
    public class RouteConfigurator
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
      

            routes.IgnoreRoute("elmah.axd");
            routes.IgnoreRoute("favicon.ico");
            routes.IgnoreRoute("robots.txt");
            routes.IgnoreRoute("AwWcfDataService.svc/{*pathInfo}");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
                , new string[] { "dFrontierAppWizard.WebUI.Controllers", "dFrontierAppWizard.WebUI.Controllers.Awesome" }
                
                );

      
        }
    }
}