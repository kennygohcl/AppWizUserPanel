using System;
using dFrontierAppWizard.Core.Model;
using dFrontierAppWizard.Core.Repository;
using dFrontierAppWizard.Core.Service;

namespace dFrontierAppWizard.Service
{
    public class ApplicationService : CrudService<Application>, IApplicationService
    {
        private readonly IFileManagerService fileManagerService;
        private readonly IRepo<MediaDocument> repoMedia ;
        private readonly IRepo<TriggerRadius> repoTriggerRadius;
        public ApplicationService(IRepo<Application> repo, IFileManagerService fileManagerService, IRepo<MediaDocument> repoMedia, IRepo<TriggerRadius> repoTriggerRadius)
            : base(repo)
        {
            this.fileManagerService = fileManagerService;
            this.repoMedia = repoMedia;
            this.repoTriggerRadius = repoTriggerRadius;
        }

        public string GetMediaValue(int id)
        {
            var value = repoMedia.Get(id);
            if (value==null)
            {
                return "";
            }
            return value.DocName;
        }

        public Decimal GetTriggerPoint(int id)
        {
            var value = repoTriggerRadius.Get(id);
            return value.Distance;
        }
        
        public void SetMediaLink(int id, string root, string filename, int x, int y, int w, int h)
        {
            fileManagerService.MakeImages(root, filename, x, y, w, h);

            var application = repo.Get(id);

            if (application.MediaLinkImage == filename) return;

            var oldMediaLinkFileName = application.MediaLinkImage;
            application.MediaLinkImage = filename;
            repo.Save();

            if (!string.IsNullOrWhiteSpace(oldMediaLinkFileName)) fileManagerService.DeleteImages(root, oldMediaLinkFileName);
        }
    }
}