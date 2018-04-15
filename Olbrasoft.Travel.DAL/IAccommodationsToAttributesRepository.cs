using System.Collections.Generic;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL
{
    public interface IAccommodationsToAttributesRepository : SharpRepository.Repository.ICompoundKeyRepository<AccommodationToAttribute, int, int, int>
    {
        void BulkSave(IEnumerable<AccommodationToAttribute> accommodationsToAttributes);
    }
}