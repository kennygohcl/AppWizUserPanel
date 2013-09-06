using System.Linq;
using System.Web.Mvc;
using Omu.AwesomeMvc;
using dFrontierAppWizard.Core.Model;
using dFrontierAppWizard.Core.Repository;

namespace dFrontierAppWizard.WebUI.Controllers.Awesome
{
    public class AdGridController : Controller
    {
        private readonly IRepo<Ad> repo;

        public AdGridController(IRepo<Ad> repo)
        {
            this.repo = repo;
        }

        public ActionResult GetItems(GridParams g, string parent)
        {
            var data = repo.Where(o => o.Application.ApplicationName.StartsWith(parent), User.IsInRole("admin"))
                .OrderByDescending(o => o.Id);

            return Json(new GridModelBuilder<Ad>(data.AsQueryable(), g)
            {
                Map = ad => new
                {
                    ad.Application.ApplicationName,
                    ad.WebSiteReference,
                    ad.Banner,
                    ad.Start,
                    ad.End,
                    Actions = this.RenderView("AdGridActions", ad) // view in Shared/AdGridActions.cshtml
                }
            }.Build());
        }
    }
}