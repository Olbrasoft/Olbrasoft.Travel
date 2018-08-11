using Olbrasoft.Travel.Data.Entity;

namespace Olbrasoft.Travel.DataAccessLayer
{
    public interface IManyToManyRepository<T> : IBulkRepository<T>, IBaseRepository<T, int, int> where T : ManyToMany
    {
    }

}