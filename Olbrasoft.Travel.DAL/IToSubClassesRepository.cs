using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL
{
    public interface IToSubClassesRepository<T> : IBaseRepository<T> where T : ToSubClass
    {
    }
}
