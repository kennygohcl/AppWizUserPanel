using System.Linq;
using System.Web.Mvc;
using Omu.AwesomeMvc;
using dFrontierAppWizard.Core.Model;
using dFrontierAppWizard.Core.Repository;

namespace dFrontierAppWizard.WebUI.Controllers.Awesome
{
    public class IndustryGridController : Controller
    {
        private readonly IRepo<Industry> repo;

        public IndustryGridController(IRepo<Industry> repo)
        {
            this.repo = repo;
        }

        public ActionResult GetItems(GridParams g, string parent)
        {
            var data = repo.Where(o => o.Description.StartsWith(parent) , User.IsInRole("admin"))
                .OrderByDescending(o => o.Id);

            return Json(new GridModelBuilder<Industry>(data.AsQueryable(), g)
            {
                Map = industry => new
                {
                    industry.Description,
                    Actions = this.RenderView("IndustryGridActions", industry) // view in Shared/IndustryGridActions.cshtml
                }
            }.Build());
        }
    }
}