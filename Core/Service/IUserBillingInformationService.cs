using dFrontierAppWizard.Core.Model;

namespace dFrontierAppWizard.Core.Service
{
    public interface IUserBillingInformationService : ICrudService<UserBillingInformation>
    {
        bool IsUnique(string login);
        bool HasBillingInfo(int id);
    }

 
}