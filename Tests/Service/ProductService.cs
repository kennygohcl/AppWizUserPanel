using DFrontried.AppWizard.Core.Model;
using DFrontried.AppWizard.Core.Repository;
using DFrontried.AppWizard.Core.Service;

namespace DFrontried.AppWizard.Service
{
    public class ProductService : CrudService<Product>, IProductService
    {
        private readonly IFileManagerService fileManagerService;

        public ProductService(IRepo<Product> repo, IFileManagerService fileManagerService)
            : base(repo)
        {
            this.fileManagerService = fileManagerService;
        }

        public void SetPicture(int id, string root, string filename, int x,int y, int w, int h)
        {
            fileManagerService.MakeImages(root, filename, x, y, w, h);
            var o = repo.Get(id);
            if (o.Picture == filename) return;
            
            var old = o.Picture;
            o.Picture = filename;
            repo.Save();

            if(!string.IsNullOrWhiteSpace(old)) fileManagerService.DeleteImages(root, old);
        }
    }
}