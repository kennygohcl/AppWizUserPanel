using dFrontierAppWizard.Core.Model;

namespace dFrontierAppWizard.Core.Service
{
    public interface IProductService : ICrudService<Product>
    {
        void SetPicture(int id, string root, string filename, int x, int y, int w, int h);
        void SetLike(int id);
    }
}