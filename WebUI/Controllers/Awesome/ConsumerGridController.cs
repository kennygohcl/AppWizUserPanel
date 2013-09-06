using System.Linq;
using System.Web.Mvc;
using Omu.AwesomeMvc;
using dFrontierAppWizard.Core.Model;
using dFrontierAppWizard.Core.Repository;

namespace dFrontierAppWizard.WebUI.Controllers.Awesome
{
    public class ConsumerGridController : Controller
    {
        private readonly IRepo<Consumer> repo;

        public ConsumerGridController(IRepo<Consumer> repo)
        {
            this.repo = repo;
        }

        public ActionResult GetItems(GridParams g, string parent)
        {
            var data = repo.Where(o => o.Name.StartsWith(parent) || o.Email.StartsWith(parent), User.IsInRole("admin"))
                .OrderByDescending(o => o.Id);
       
            return Json(new GridModelBuilder<Consumer>(data.AsQueryable(), g)
            {
                Map = con => new
                {
                    con.Name,
                    con.Age,
                    con.BirthDate,
                    con.Gender,
                    con.Email,
                    con.Phone,
                    con.Address,
                    con.DateRegistered//,
                   // Actions = this.RenderView("ConsumerGridActions", con) // view in Shared/ConsumerGridActions.cshtml
                }
            }.Build());
        }
    }
}