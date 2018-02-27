using Olbrasoft.Travel.DAL;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.BLL
{
    public class StatusesFacade : TravelFacade<Status>, IStatusesFacade {
        public StatusesFacade(ITravelRepository<Status> repository) : base(repository)
        {
        }

        public void AddIfNotExist(ref Status status)
        {
            var statusIn = status;
            var storedStatus = Repository.Find(s => s.Id == statusIn.Id || s.Name == statusIn.Name);
            if (storedStatus == null)
            {
                Repository.Add(status);
            }
            else
            {
                status = storedStatus;
            }
        }
    }
}