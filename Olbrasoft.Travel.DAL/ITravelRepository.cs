namespace Olbrasoft.Travel.DAL
{
    public interface ITravelRepository<T> : SharpRepository.Repository.IRepository<T> where T : class
    {

    }
}
