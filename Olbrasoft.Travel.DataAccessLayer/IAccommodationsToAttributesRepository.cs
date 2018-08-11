using System.Collections.Generic;
using Olbrasoft.Travel.Data.Entity;

namespace Olbrasoft.Travel.DataAccessLayer
{
    public interface IAccommodationsToAttributesRepository : SharpRepository.Repository.ICompoundKeyRepository<AccommodationToAttribute, int, int, int>
    {
        void BulkSave(IEnumerable<AccommodationToAttribute> accommodationsToAttributes);
    }
}