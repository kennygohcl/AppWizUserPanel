using System;
using System.Linq;
using Omu.Encrypto;
using dFrontierAppWizard.Core.Model;
using dFrontierAppWizard.Core.Repository;
using dFrontierAppWizard.Core.Service;

namespace dFrontierAppWizard.Service
{
    public class ConsumerBillingInformationService : CrudService<ConsumerBillingInformation>, IConsumerBillingInformationService
    {

        public ConsumerBillingInformationService(IRepo<ConsumerBillingInformation> repo)
            : base(repo)
        {
          
        }

        public override int Create(ConsumerBillingInformation consumerBillingInformation)
        {

            return base.Create(consumerBillingInformation);
        }

        public bool IsUnique(string login)
        {
            return repo.Where(o => o.FirstName == login).Count() == 0;
        }

        public bool HasBillingInfo(int id)
        {
            return repo.Where(o => o.ConsumerId==id).Count() > 0;
        }

       
        
    }
}