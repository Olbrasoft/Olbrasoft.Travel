using Olbrasoft.Travel.Data.Entity;

namespace Olbrasoft.Travel.BusinessLogicLayer
{
    public interface ILanguagesFacade : ITravelFacade<Language>
    {
        Language Get(int id);
    }
}
