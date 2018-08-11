using Olbrasoft.Travel.Data.Entity;
using Olbrasoft.Travel.DataAccessLayer;

namespace Olbrasoft.Travel.BusinessLogicLayer
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
