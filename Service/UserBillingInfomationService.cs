using System;
using System.Linq;
using Omu.Encrypto;
using dFrontierAppWizard.Core.Model;
using dFrontierAppWizard.Core.Repository;
using dFrontierAppWizard.Core.Service;

namespace dFrontierAppWizard.Service
{
    public class UserBillingInformationService : CrudService<UserBillingInformation>, IUserBillingInformationService
    {
     
        public UserBillingInformationService(IRepo<UserBillingInformation> repo)
            : base(repo)
        {
          
        }

        public override int Create(UserBillingInformation userBillingInformation)
        {

            return base.Create(userBillingInformation);
        }

        public bool IsUnique(string login)
        {
            return repo.Where(o => o.FirstName == login).Count() == 0;
        }

        public bool HasBillingInfo(int id)
        {
            return repo.Where(o => o.UserId==id).Count() > 0;
        }

       
        
    }
}