using Olbrasoft.Travel.Data.Entity;

namespace Olbrasoft.Travel.DataAccessLayer
{
    public interface IUsersRepository:IBaseRepository<User>
    {
        User AddIfNotExist( User user);
    }
}
