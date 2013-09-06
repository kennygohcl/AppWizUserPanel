using System.Linq;
using System.Web.Mvc;
using Omu.AwesomeMvc;
using dFrontierAppWizard.Core.Model;
using dFrontierAppWizard.Core.Repository;

namespace dFrontierAppWizard.WebUI.Controllers.Awesome
{
    public class LeadGridController : Controller
    {
        private readonly IRepo<Lead> repo;

        public LeadGridController(IRepo<Lead> repo)
        {
            this.repo = repo;
        }

        public ActionResult GetItems(GridParams g, string parent)
        {
            var data = repo.Where(o => o.FirstName.StartsWith(parent) || o.LastName.StartsWith(parent), User.IsInRole("admin"))
                .OrderByDescending(o => o.Id);

            return Json(new GridModelBuilder<Lead>(data.AsQueryable(), g)
            {
                Map = lead => new
                {
                    lead.FirstName,
                    lead.LastName,
                    lead.Address,
                    lead.EmailAddress,
                    lead.Mobile,
                    lead.ReferredBy,
                    lead.DateCreated,
                    Actions = this.RenderView("LeadGridActions", lead) // view in Shared/LeadGridActions.cshtml
                }
            }.Build());
        }
    }
}