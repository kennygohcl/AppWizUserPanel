using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Omu.AwesomeMvc;
using dFrontierAppWizard.Core.Model;
using dFrontierAppWizard.Core.Repository;
using dFrontierAppWizard.Resources;

namespace dFrontierAppWizard.WebUI.Controllers
{
    public class FeedbackGiftImageIdAjaxDropdownController : Controller
    {
        private IRepo<MediaDocument> repo;
        public FeedbackGiftImageIdAjaxDropdownController(IRepo<MediaDocument> repo)
        {
            this.repo = repo;
        }

        public ActionResult GetItems(int? v, int? appId)
        {
            var list = new List<SelectableItem> { new SelectableItem { Text = Mui.not_selected, Value = "" } };

            list.AddRange(repo.Where(o => o.ApplicationId == appId && o.MediaType == "Image" && o.Module=="Feedback" ).ToArray().Select(o => new SelectableItem
            {
                Text = string.Format("{0}", o.DocName),
                Value = o.Id.ToString(),
                Selected = o.Id == v
            }));

            return Json(list);
        }
    }
}