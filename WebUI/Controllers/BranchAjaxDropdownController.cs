using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Omu.AwesomeMvc;
using dFrontierAppWizard.Core.Model;
using dFrontierAppWizard.Core.Repository;
using dFrontierAppWizard.Core.Service;
using dFrontierAppWizard.Resources;

namespace dFrontierAppWizard.WebUI.Controllers
{
    public class BranchIdAjaxDropdownController : Controller
    {
        private readonly IRepo<Branch> repo;
        private new readonly IUserService userService;
        public BranchIdAjaxDropdownController(IRepo<Branch> repo, IUserService userService)
        {
            this.repo = repo;
            this.userService = userService;
        }

        public ActionResult GetItems(int? v)
        {
            var list = new List<SelectableItem> { new SelectableItem ("", Mui.not_selected) };
            int userId = userService.GetUserIdByName(HttpContext.User.Identity.Name);

            list.AddRange(repo.Where(o => o.UserId == userId).ToArray().Select(o => new SelectableItem
                                                     {
                                                         Text = string.Format("{0} {1}", o.BranchName, o.Address),
                                                         Value = o.Id.ToString(),
                                                         Selected = o.Id == v
                                                     }));
            return Json(list);
        }
    }
}