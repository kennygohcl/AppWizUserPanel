using dFrontierAppWizard.Core.Model;

namespace dFrontierAppWizard.Core.Service
{
    public interface IUserService : ICrudService<User>
    {
        bool IsUnique(string login);
        void ChangePassword(int id, string password);
        User Get(string Login, string password);
        int GetUserIdByName(string loginName);
        int GetUserTypeId(string loginName);
    }

 
}