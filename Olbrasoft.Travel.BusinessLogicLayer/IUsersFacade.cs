using Olbrasoft.Travel.Data.Entity;

namespace Olbrasoft.Travel.BusinessLogicLayer
{
    public interface IUsersFacade:ITravelFacade<User>
    {
        void AddIfNotExist(ref User user);
    }


}
