using System.Linq;
using System.Web.Mvc;
using Omu.AwesomeMvc;
using dFrontierAppWizard.Core.Model;
using dFrontierAppWizard.Core.Repository;
using dFrontierAppWizard.Core.Service;

namespace dFrontierAppWizard.WebUI.Controllers.Awesome
{
    public class TransactionGridController : Controller
    {
        private readonly IRepo<Transaction> repo;
        private new readonly IUserService userService;
        public TransactionGridController(IRepo<Transaction> repo, IUserService userService)
        {
            this.repo = repo;
            this.userService = userService;
        }

        public ActionResult GetItems(GridParams g, string parent)
        {
            int userId = userService.GetUserIdByName(HttpContext.User.Identity.Name) == 0 ? 0 : userService.GetUserIdByName(HttpContext.User.Identity.Name);
            var data = repo.GetAll()
                .OrderByDescending(o => o.Id);

            return Json(new GridModelBuilder<Transaction>(data.AsQueryable(), g)
            {
                Map = trans => new
                {
                    trans.DateofTransaction,
                    trans.DiscountOfTotalPercent,
                    trans.FinalAmount,
                    trans.TotalAmount,
                    trans.PaymentReference,
                    trans.Status,
                    Actions = this.RenderView("TransactionGridActions", trans) // view in Shared/TransactionGridActions.cshtml
                }
            }.Build());
        }
    }
}