using System.Collections.Generic;
using Olbrasoft.Travel.DAL;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.BLL
{
    public class ContinentsFacade : TravelFacade<Continent>,IContinentsFacade
    {
        protected new readonly IContinentsRepository Repository;

        public ContinentsFacade(IContinentsRepository repository) : base(repository)
        {
            Repository = repository;
        }
    }
}