using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Omu.AwesomeMvc;
using dFrontierAppWizard.Core.Model;
using dFrontierAppWizard.Core.Repository;

namespace dFrontierAppWizard.WebUI.Controllers
{
    public class IndustriesMultiLookupController : Controller
    {
        private readonly IRepo<Industry> repo;

        public IndustriesMultiLookupController(IRepo<Industry> repo)
        {
            this.repo = repo;
        }

        [HttpPost]
        public ActionResult Search(string search, int[] selected, int page)
        {
            const int PageSize = 9;
            var list = repo.Where(o => o.Description.Contains(search));

            if (selected != null) list = list.Where(o => !selected.Contains(o.Id)).AsQueryable();
            list = list.OrderByDescending(o => o.Id);

            return Json(new AjaxListResult
                            {
                                Content = this.RenderView("ListItems/IndustryItem", list.Page(page, PageSize).ToList()),
                                More = list.Count() > page * PageSize
                            });
        }

        public ActionResult Selected(IEnumerable<int> selected)
        {
            var items = (selected != null)
                            ? repo.GetAll().Where(o => selected.Contains(o.Id)).ToList()
                            : new List<Industry>();

            return Json(new AjaxListResult
                         {
                             Content = this.RenderView("ListItems/IndustryItem", items)
                         });
        }

        public ActionResult GetItems(IEnumerable<int> v)
        {
            return Json(repo.GetAll().Where(o => v.Contains(o.Id)).ToArray().Select(o => new KeyContent
            {
                Key = o.Id,
                Content =  o.Description,
                Encode = false
            }));
        }

        

    }
}