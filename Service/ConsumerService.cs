using System;
using System.Linq;
using Omu.Encrypto;
using dFrontierAppWizard.Core.Model;
using dFrontierAppWizard.Core.Repository;
using dFrontierAppWizard.Core.Service;

namespace dFrontierAppWizard.Service
{
    public class ConsumerService : CrudService<Consumer>, IConsumerService
    {
       
        public ConsumerService(IRepo<Consumer> repo, IHasher hasher)
            : base(repo)
        {
            hasher.SaltSize = 10;
        }

        public override int Create(Consumer consumer)
        {
            return base.Create(consumer);
        }

        public bool IsUnique(string email)
        {
            return repo.Where(o => o.Email == email).Count() == 0;
        }

        public Consumer Get(string email)
        {
            var consumer = repo.Where(o => o.Email== email && o.IsDeleted == false).SingleOrDefault();
            return consumer;
        }
        
    }
}