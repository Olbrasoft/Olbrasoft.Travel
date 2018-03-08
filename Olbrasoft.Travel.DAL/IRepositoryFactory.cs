using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL
{
    public interface IRepositoryFactory
    {
        ITravelRepository<T> Travel<T>() where T : class;

        IKeyIdRepository<T> KeyId<T>() where T : class, IKeyId;

        IRegionsRepository CreateRegionsRepository();
        
    }
}
