using Olbrasoft.Travel.DAL;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.BLL
{
    public class ImportsFacade : TravelFacade<Import>, IImportsFacade
    {
        protected new readonly IImportsRepository Repository;
       
        public ImportsFacade(IImportsRepository importsRepository) : base(importsRepository)
        {
            Repository = importsRepository;
            
        }

        public Import StarImport(string userName, string description)
        {

            var user = new User{UserName = userName};
            var status= new Status(){Name = "Start",Creator = user};
            var import = new Import() {Creator=user, Description = description, CurrentStatus = status};
            Repository.Add(import);
            return import;
        }
        
        

    }
}