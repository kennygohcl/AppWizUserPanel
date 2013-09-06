using System.Linq;
using System.Web.Mvc;
using Omu.AwesomeMvc;
using dFrontierAppWizard.Core.Model;
using dFrontierAppWizard.Core.Repository;

namespace dFrontierAppWizard.WebUI.Controllers.Awesome
{
    public class MediaDocumentGridController : Controller
    {
        private readonly IRepo<MediaDocument> repo;

        public MediaDocumentGridController(IRepo<MediaDocument> repo)
        {
            this.repo = repo;
        }

        public ActionResult GetItems(GridParams g, string parent)
        {
            var data = repo.Where(o => o.DocName.StartsWith(parent) || o.MediaType.StartsWith(parent), User.IsInRole("admin"))
                .OrderByDescending(o => o.Id);

            return Json(new GridModelBuilder<MediaDocument>(data.AsQueryable(), g)
            {
                Map = mediaDocument => new
                {
                    mediaDocument.DocName,
                    mediaDocument.MediaType,
                    mediaDocument.Module,
                    mediaDocument.DateCreated,
                    Actions = this.RenderView("MediaDocumentGridActions", mediaDocument) // view in Shared/MediaDocumentActions.cshtml
                }
            }.Build());
        }
    }
}