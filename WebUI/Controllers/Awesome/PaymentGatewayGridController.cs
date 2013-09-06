using System.Linq;
using System.Web.Mvc;
using Omu.AwesomeMvc;
using dFrontierAppWizard.Core.Model;
using dFrontierAppWizard.Core.Repository;

namespace dFrontierAppWizard.WebUI.Controllers.Awesome
{
    public class PaymentGatewayGridController : Controller
    {
        private readonly IRepo<PaymentGateway> repo;

        public PaymentGatewayGridController(IRepo<PaymentGateway> repo)
        {
            this.repo = repo;
        }

        public ActionResult GetItems(GridParams g, string parent)
        {
            var data = repo.Where(o => o.ApiLogin.StartsWith(parent), User.IsInRole("admin"))
                .OrderByDescending(o => o.Id);

            return Json(new GridModelBuilder<PaymentGateway>(data.AsQueryable(), g)
            {
                Map = pg => new
                {
                    pg.ApiLogin,
                    pg.TransactionKey,
                    pg.Type,
                    Actions = this.RenderView("PaymentGatewayGridActions", pg) // view in Shared/PaymentGatewayGridActions.cshtml
                }
            }.Build());
        }
    }
}