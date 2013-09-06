using System.Linq;
using System.Web.Mvc;
using Omu.AwesomeMvc;
using dFrontierAppWizard.Core.Model;
using dFrontierAppWizard.Core.Repository;
using dFrontierAppWizard.WebUI.Controllers.Awesome;

namespace dFrontierAppWizard.WebUI.Controllers
{
    public class CountryIdLookupController : Controller
    {
        private readonly IRepo<Country> repo;

        public CountryIdLookupController(IRepo<Country> repo)
        {
            this.repo = repo;
        }

        public ActionResult SearchForm()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(string search, int page)
        {
            var list = repo.Where(o => o.Name.StartsWith(search), User.IsInRole("admin"))
                .OrderByDescending(o => o.Id);

            return Json(new AjaxListResult
            {
                Content = this.RenderView("ListItems/Country", list.Page(page, 10).ToList()),
                More = list.Count() > page * 10
            });
        }

        public ActionResult GetItem(int v)
        {
            var c = repo.Get(v) ?? new Country();
            return Json(new KeyContent(c.Id, c.Name));
        }
    }
}