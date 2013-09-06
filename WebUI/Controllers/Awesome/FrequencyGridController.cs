using System;
using System.Linq;
using System.Web.Mvc;
using Omu.AwesomeMvc;
using dFrontierAppWizard.Core.Model;
using dFrontierAppWizard.Core.Repository;

namespace dFrontierAppWizard.WebUI.Controllers.Awesome
{
    public class FrequencyGridController : Controller
    {
        private readonly IRepo<Frequency> repo;

        public FrequencyGridController(IRepo<Frequency> repo)
        {
            this.repo = repo;
        }

        public ActionResult GetItems(GridParams g, string parentSearch, string parent)
        {
             

            var data = repo.Where(o => o.ApplicationId.Equals(0) && o.Description.Contains(parent), User.IsInRole("admin"))
              .OrderByDescending(o => o.Id);

            int j;
            bool result = Int32.TryParse(parentSearch, out j);
            if (true == result)
            {
                data = repo.Where(o => o.ApplicationId.Equals(j) && o.Description.Contains(parent), User.IsInRole("admin"))
               .OrderByDescending(o => o.Id);
            }
           
            return Json(new GridModelBuilder<Frequency>(data.AsQueryable(), g)
            {
                Map = freq => new
                {
                    freq.Description,
                    Actions = this.RenderView("FrequencyGridActions", freq)
                }
            }.Build());
        }

        public ActionResult GetItemsNotDeleted(GridParams g, string parentSearch)
        {
            var data = repo.Where(o => o.ApplicationId.Equals(0), User.IsInRole("admin"))
              .OrderByDescending(o => o.Id);

            int j;
            bool result = Int32.TryParse(parentSearch, out j);
            if (true == result)
            {
                data = repo.Where(o => o.ApplicationId.Equals(j) && o.IsDeleted==false, User.IsInRole("admin"))
               .OrderByDescending(o => o.Id);
            }

            return Json(new GridModelBuilder<Frequency>(data.AsQueryable(), g)
            {
                Map = freq => new
                {
                    freq.Description
                }
            }.Build());
        }
    }
}