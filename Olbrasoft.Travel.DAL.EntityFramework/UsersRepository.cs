using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class UsersRepository : BaseRepository<User>, IUsersRepository
    {
        //protected readonly TravelContext Context;

        public UsersRepository(DbContext context) : base(context)
        {

        }

        

        public User AddIfNotExist(User user)
        {
            var userIn = user;
            var storedUser = Find(u => u.Id == userIn.Id || u.UserName == userIn.UserName);

            if (storedUser == null)
            {
                Add(user);
            }
            else
            {
                user = storedUser;
            }

            return user;
        }
    }
}
