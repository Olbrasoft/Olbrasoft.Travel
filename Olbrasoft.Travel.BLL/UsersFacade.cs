using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Olbrasoft.Travel.DAL;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.BLL
{
    public class UsersFacade:TravelFacade<User>,IUsersFacade
    {
        protected new readonly IUsersRepository Repository;

        public UsersFacade(IUsersRepository usersRepository) : base(usersRepository)
        {
            Repository = usersRepository;
        }

        public void AddIfNotExist(ref User user)
        {
            var userIn = user;
            var storedUser = Repository.Find(u => u.Id == userIn.Id || u.UserName == userIn.UserName);

            if (storedUser == null)
            {
                Repository.Add(user);
            }
            else
            {
                user = storedUser;
            }
        }
    }
}
