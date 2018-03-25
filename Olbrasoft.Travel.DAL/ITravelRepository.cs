using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL
{
    public interface ITravelRepository<T> : IBaseRepository<T>, IBulkRepository<T> where T : CreationInfo
    {

    }
}