using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL
{
    public interface IUsersRepository:IBaseRepository<User>
    {
        void AddIfNotExist(ref User user);
    }
}
