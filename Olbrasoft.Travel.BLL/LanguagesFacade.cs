using Olbrasoft.Travel.DAL;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.BLL
{
    public class LanguagesFacade : TravelFacade<Language>, ILanguagesFacade
    {
        protected new readonly ILanguagesRepository Repository;

        public LanguagesFacade(ILanguagesRepository repository) : base(repository)
        {
            Repository = repository;
        }

        public Language Get(int id)
        {
            return Repository.Get(id);
        }
    }
}