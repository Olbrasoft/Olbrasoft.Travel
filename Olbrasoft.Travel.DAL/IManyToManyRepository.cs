using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL
{
    public interface IManyToManyRepository<T> : IBulkRepository<T>, IBaseRepository<T, int, int> where T : ManyToMany
    {
        
    }
    
}