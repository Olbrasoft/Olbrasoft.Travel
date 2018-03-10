using System;
using System.Linq;
using System.Linq.Expressions;
using Olbrasoft.Travel.DAL;
using Olbrasoft.Travel.DTO;
using SharpRepository.Repository.FetchStrategies;
using SharpRepository.Repository.Queries;

namespace Olbrasoft.Travel.BLL
{
    public class TypesOfRegionsFacade : TravelFacade<TypeOfRegion>, ITypesOfRegionsFacade {
        public TypesOfRegionsFacade(IBaseRepository<TypeOfRegion> repository) : base(repository)
        {

        }

        public TypeOfRegion Get(string name, params Expression<Func<TypeOfRegion, object>>[] includePaths)
        {
            return Repository.Find(typeOfRegion => typeOfRegion.Name == name, includePaths);

        }

    }
}