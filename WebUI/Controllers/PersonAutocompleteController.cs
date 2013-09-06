using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Omu.AwesomeMvc;
using cm=dFrontierAppWizard.Core.Model;
using dFrontierAppWizard.Core.Repository;
using dFrontierAppWizard.Core.Service;

namespace dFrontierAppWizard.WebUI.Controllers
{
    public class PromotionAutocompleteController : Controller
    {
        private readonly IRepo<cm.Promotion> repo;
        private new readonly IUserService userService;
        private readonly IRepo<cm.Product> repoProduct;
        private readonly IRepo<cm.ProductCategory> repoProductCategory;
        public PromotionAutocompleteController(IRepo<cm.Promotion> repo, IUserService userService, IRepo<cm.Product> repoProduct, IRepo<cm.ProductCategory> repoProductCategory)
        {
            this.repo = repo;
            this.userService = userService;
            this.repoProduct = repoProduct;
            this.repoProductCategory = repoProductCategory;
        }

        public ActionResult GetItems(string v, int? productCategory, IEnumerable<int> products)
        {
            var query = repo.Where(o => o.Description.Contains(v));
            if (productCategory.HasValue)
            {
                query = query.Where(o => o.Products.All(m => m.ProductCategoryId.Equals(productCategory)));
            }
            
            if (products != null)
            {
                query = query.Where(o => products.All(m => o.Products.Select(g => g.Id).Contains(m)));
            }
            else
            {

                int userId = userService.GetUserIdByName(HttpContext.User.Identity.Name) == 0 ? 0 : userService.GetUserIdByName(HttpContext.User.Identity.Name);
                query = query.Where(o => o.Products.All(m => m.Userid.Equals(userId)));
            }

            var list = query.ToList();

            return Json(list.Select(i => new KeyContent { Content = i.Description, Key = i.Id, Encode = false}).Take(5));
        }
    }
}