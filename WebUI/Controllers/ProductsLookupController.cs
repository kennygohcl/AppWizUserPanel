using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Omu.AwesomeMvc;
using dFrontierAppWizard.Core.Model;
using dFrontierAppWizard.Core.Repository;
using dFrontierAppWizard.Core.Service;

namespace dFrontierAppWizard.WebUI.Controllers
{
    public class ProductsMultiLookupController : Controller
    {
        private readonly IRepo<Product> repo;
        private new readonly IUserService userService;

        public ProductsMultiLookupController(IRepo<Product> repo, IUserService userService)
        {
            this.repo = repo;
            this.userService = userService;
        }

        [HttpPost]
        public ActionResult Search(string search, int[] selected, int page)
        {
            const int PageSize = 9;
            var list = repo.Where(o => o.Name.Contains(search));

            int userId = userService.GetUserIdByName(HttpContext.User.Identity.Name) == 0
                         ? 0
                         : userService.GetUserIdByName(HttpContext.User.Identity.Name);

            if (selected != null) list = list.Where(o => !selected.Contains(o.Id)).AsQueryable();

            list = list.Where(o => o.Userid.Equals(userId));

            list = list.OrderByDescending(o => o.Id);

            return Json(new AjaxListResult
                            {
                                Content = this.RenderView("ListItems/ProductItem", list.Page(page, PageSize).ToList()),
                                More = list.Count() > page * PageSize
                            });
        }

        public ActionResult Selected(IEnumerable<int> selected)
        {
            var items = (selected != null)
                            ? repo.GetAll().Where(o => selected.Contains(o.Id)).ToList()
                            : new List<Product>();

            return Json(new AjaxListResult
                         {
                             Content = this.RenderView("ListItems/ProductItem", items)
                         });
        }

        public ActionResult GetItems(IEnumerable<int> v)
        {
            return Json(repo.GetAll().Where(o => v.Contains(o.Id)).ToArray().Select(o => new KeyContent
            {
                Key = o.Id,
                Content = @"<img  src='" + Url.Content("~/pictures/Products/" + Pic(o.Picture)) + "' class='mthumb' />" + o.Name,
                Encode = false
            }));
        }

        private static string Pic(string o)
        {
            return string.IsNullOrEmpty(o) ? "m0.jpg" : "m" + o;
        }

    }
}