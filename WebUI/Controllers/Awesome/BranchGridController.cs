using System.Linq;
using System.Web.Mvc;
using Omu.AwesomeMvc;
using dFrontierAppWizard.Core.Model;
using dFrontierAppWizard.Core.Repository;
using dFrontierAppWizard.Core.Service;

namespace dFrontierAppWizard.WebUI.Controllers.Awesome
{
    public class BranchGridController : Controller
    {
        private readonly IRepo<Branch> repo;
        private new readonly IUserService userService;
        public BranchGridController(IRepo<Branch> repo, IUserService userService)
        {
            this.repo = repo;
            this.userService = userService;
        }

        public ActionResult GetItems(GridParams g, string parent)
        {
            var data = repo.Where(o => o.BranchName.StartsWith(parent) || o.Address.StartsWith(parent), User.IsInRole("admin"))
                .OrderByDescending(o => o.Id);
            
            return Json(new GridModelBuilder<Branch>(data.AsQueryable(), g)
            {
                Map = branch => new
                {
                    branch.BranchName,
                    branch.Address,
                    branch.ZipCode,
                    Country = branch.Country.Name,
                    Actions = this.RenderView("BranchGridActions", branch) // view in Shared/BranchGridActions.cshtml
                }
            }.Build());
        }

        public ActionResult GetItemsAllByUserId(GridParams g)
        {
            int userId = userService.GetUserIdByName(HttpContext.User.Identity.Name) == 0
                           ? 0
                           : userService.GetUserIdByName(HttpContext.User.Identity.Name);

            var data = repo.Where(o => o.UserId == userId, User.IsInRole("admin"))
                .OrderByDescending(o => o.Id);

            return Json(new GridModelBuilder<Branch>(data.AsQueryable(), g)
            {
                Map = branch => new
                {
                    branch.BranchName,
                    branch.Address,
                    branch.ZipCode,
                    Country = branch.Country.Name,
                    Actions = this.RenderView("BranchGridActions", branch) // view in Shared/BranchGridActions.cshtml
                }
            }.Build());
        }
    }
}