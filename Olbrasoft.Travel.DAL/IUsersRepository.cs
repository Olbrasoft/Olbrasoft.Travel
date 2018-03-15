using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL
{
    public interface IUsersRepository:IBaseRepository<User>
    {
        User AddIfNotExist( User user);
    }
}
