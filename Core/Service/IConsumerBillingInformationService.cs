using dFrontierAppWizard.Core.Model;

namespace dFrontierAppWizard.Core.Service
{
    public interface IConsumerBillingInformationService : ICrudService<ConsumerBillingInformation>
    {
        bool IsUnique(string login);
        bool HasBillingInfo(int id);
    }

 
}