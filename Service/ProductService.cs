using dFrontierAppWizard.Core.Model;
using dFrontierAppWizard.Core.Repository;
using dFrontierAppWizard.Core.Service;

namespace dFrontierAppWizard.Service
{
    public class ProductService : CrudService<Product>, IProductService
    {
        private readonly IFileManagerService fileManagerService;

        public ProductService(IRepo<Product> repo, IFileManagerService fileManagerService) : base(repo)
        {
            this.fileManagerService = fileManagerService;
        }

        public void SetPicture(int id, string root, string filename, int x,int y, int w, int h)
        {
            fileManagerService.MakeImages(root, filename, x, y, w, h);

            var product = repo.Get(id);

            if (product.Picture == filename) return;

            var oldPictureFileName = product.Picture;
            product.Picture = filename;
            repo.Save();

            if(!string.IsNullOrWhiteSpace(oldPictureFileName)) fileManagerService.DeleteImages(root, oldPictureFileName);
        }

        public void SetLike(int id)
        {
            var product = repo.Get(id);
            
            var productLikes = product.LikesCounter+1;
            product.LikesCounter = productLikes;
            repo.Save();

        }
    }
}