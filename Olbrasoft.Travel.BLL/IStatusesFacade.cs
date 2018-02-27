using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.BLL
{
    public interface IStatusesFacade : ITravelFacade<Status>
    {
        void AddIfNotExist(ref Status status);
    }
}