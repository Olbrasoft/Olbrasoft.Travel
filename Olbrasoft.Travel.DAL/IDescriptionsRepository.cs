using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL
{
    public interface IDescriptionsRepository : IBulkRepository<Description>, SharpRepository.Repository.ICompoundKeyRepository<Description, int, int, int>
    {

    }
}