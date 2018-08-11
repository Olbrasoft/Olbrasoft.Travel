 using System;
using System.Collections.Generic;
 using System.Data.Entity;
 using System.Linq;
using System.Web;
using System.Web.Mvc;
 using Olbrasoft.Travel.DataAccessLayer;
 using Olbrasoft.Travel.Data.Entity;



namespace Olbrasoft.Travel.Web.Mvc.Controllers
{

    /// <summary>
    /// Install-Package Castle.Windsor.Web.Mvc -Version 1.0.7
    ///  Install-Package EntityFramework
    /// </summary>
    public class HomeController : Controller
    {
        private readonly IMappedEntitiesRepository<Accommodation> _repository;

        public HomeController(IMappedEntitiesRepository<Accommodation> repository)
        {
            this._repository = repository;
        }

        // GET: Home
        public ActionResult Index()
        {
            return View(_repository.AsQueryable().Take(5).Include(p => p.LocalizedAccommodations).ToList());
        }
    }
}