using System.Linq;
using System.Web.Mvc;
using Omu.AwesomeMvc;
using cm=dFrontierAppWizard.Core.Model;
using dFrontierAppWizard.Core.Repository;
using dFrontierAppWizard.Core.Service;

namespace dFrontierAppWizard.WebUI.Controllers.Awesome
{
    public class PromotionGridController : Controller
    {
        private readonly IRepo<cm.Promotion> repo;
        private new readonly IUserService userService;
        private readonly IRepo<cm.Product> repoProduct;
        public PromotionGridController(IRepo<cm.Promotion> repo, IUserService userService, IRepo<cm.Product> repoProduct)
        {
            this.repo = repo;
            this.userService = userService;
            this.repoProduct = repoProduct;
        }

        public ActionResult GetItems(GridParams g, string search, int? productCategory, int[] products)
        {
            g.PageSize = 10;
            var list = repo.Where(o => o.Description.Contains(search), User.IsInRole("admin"));

            if (productCategory.HasValue)
            {
                list = list.Where(o => o.Products.All(m => m.ProductCategoryId.Equals(productCategory)));
            }
            if (products != null)
            {
                list = list.Where(o => products.All(m => o.Products.Select(product => product.Id).Contains(m)));
            }
            else
            {
                int userId = userService.GetUserIdByName(HttpContext.User.Identity.Name) == 0 ? 0 : userService.GetUserIdByName(HttpContext.User.Identity.Name);
                list = list.Where(o => o.Products.All(m => m.Userid.Equals(userId)));
            }

      
            //by default ordering by id
            list = list.OrderByDescending(o => o.Id);

            return Json(new GridModelBuilder<cm.Promotion>(list.AsQueryable(), g)
                {
                    // Key = "Id", this is needed for EF to always sort by something, but we already do order by Id
                    Map = o => new
                        {
                            o.Id,
                            o.IsDeleted,
                            o.Description,
                            ProductsCount = o.Products.Count,
                        }
                }.Build());
        }
    }
}