using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.BLL
{
    public interface ILocalizedFacade
    {
        bool Exists<T>(Language language);
    }
}
