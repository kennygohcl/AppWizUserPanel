using System;
using System.Linq;
using Omu.Encrypto;
using dFrontierAppWizard.Core.Model;
using dFrontierAppWizard.Core.Repository;
using dFrontierAppWizard.Core.Service;

namespace dFrontierAppWizard.Service
{
    public class UserService : CrudService<User>, IUserService
    {
        private readonly IHasher hasher;

        public UserService(IRepo<User> repo, IHasher hasher)
            : base(repo)
        {
            this.hasher = hasher;
            hasher.SaltSize = 10;
        }

        public override int Create(User user)
        {
            user.Password = hasher.Encrypt(user.Password);
            return base.Create(user);
        }

        public bool IsUnique(string login)
        {
            return repo.Where(o => o.Login == login).Count() == 0;
        }

        public User Get(string login, string password)
        {
            var user = repo.Where(o => o.Login == login && o.IsDeleted == false).SingleOrDefault();
            if (user == null || !hasher.CompareStringToHash(password, user.Password)) return null;
            return user;
        }

        public void ChangePassword(int id, string password)
        {
            repo.Get(id).Password = hasher.Encrypt(password);
            repo.Save();
        }

        public int GetUserIdByName(string loginName)
        {
            var user = repo.Where(o => o.Login == loginName).SingleOrDefault();
            if (user==null)
            {
                return 0;
            }
            else
            {
                return user.Id;
            }
           
        }

        public int GetUserTypeId(string loginName)
        {
            var user = repo.Where(o => o.Login == loginName).SingleOrDefault();
            if (user==null)
            {
                return 0;
            }
            else
            {
                return user.UserTypeId;
            }

        }
    }
}