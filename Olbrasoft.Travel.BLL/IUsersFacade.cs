using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.BLL
{
    public interface IUsersFacade:ITravelFacade<User>
    {
        void AddIfNotExist(ref User user);
    }


}
