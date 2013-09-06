using System.Linq;
using System.Web.Mvc;
using Omu.AwesomeMvc;
using dFrontierAppWizard.Core.Model;
using dFrontierAppWizard.Core.Repository;

namespace dFrontierAppWizard.WebUI.Controllers.Awesome
{
    public class ConsumerFeedbackGridController : Controller
    {
        private readonly IRepo<ConsumerFeedback> repo;

        public ConsumerFeedbackGridController(IRepo<ConsumerFeedback> repo)
        {
            this.repo = repo;
        }

        public ActionResult GetItems(GridParams g, string parent)
        {
            var data = repo.Where(o => o.Comments.StartsWith(parent) && o.IsDeleted==false)
                .OrderByDescending(o => o.Id);

            return Json(new GridModelBuilder<ConsumerFeedback>(data.AsQueryable(), g)
            {
                Map = consumerFeedback => new
                {   
                    consumerFeedback.Consumer.Name,
                    consumerFeedback.Comments,
                    consumerFeedback.DatePosted,
                    Actions = this.RenderView("ConsumerFeedbackGridActions", consumerFeedback) // view in Shared/ConsumerFeedbackGridActions.cshtml
                }
            }.Build());
        }
    }
}
