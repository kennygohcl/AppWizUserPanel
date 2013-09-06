using dFrontierAppWizard.Core.Model;
namespace dFrontierAppWizard.Core.Service
{
    public interface IConsumerService : ICrudService<Consumer>
    {
        bool IsUnique(string email);
        Consumer Get(string email);
    }
}



