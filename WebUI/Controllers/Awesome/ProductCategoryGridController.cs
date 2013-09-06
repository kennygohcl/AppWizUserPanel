using System.Linq;
using System.Web.Mvc;
using Omu.AwesomeMvc;
using dFrontierAppWizard.Core.Model;
using dFrontierAppWizard.Core.Repository;
using dFrontierAppWizard.Core.Service;

namespace dFrontierAppWizard.WebUI.Controllers.Awesome
{
    public class ProductCategoryGridController : Controller
    {
        private readonly IRepo<ProductCategory> repo;
        private new readonly IUserService userService;
        public ProductCategoryGridController(IRepo<ProductCategory> repo, IUserService userService)
        {
            this.repo = repo;
            this.userService = userService;
        }

        public ActionResult GetItems(GridParams g, string parent)
        {
            int userId = userService.GetUserIdByName(HttpContext.User.Identity.Name) == 0 ? 0 : userService.GetUserIdByName(HttpContext.User.Identity.Name);

            var data = repo.Where(o => o.Description.StartsWith(parent) && o.UserId.Equals(userId), User.IsInRole("admin"))
                .OrderByDescending(o => o.Id);
 
            return Json(new GridModelBuilder<ProductCategory>(data.AsQueryable(), g)
            {
                Map = productCategory => new
                {
                    productCategory.Description,
                    Actions = this.RenderView("ProductCategoryGridActions", productCategory) // view in Shared/ProductCategoryGridActions.cshtml
                }
            }.Build());
        }
    }
}