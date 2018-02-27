using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class UsersRepository : TravelRepository<User>,IUsersRepository
    {
        public UsersRepository(TravelContext travelContext) : base(travelContext)
        {
        }
    }
}
