using System.Data.Entity;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Olbrasoft.Travel.Data.Entity;
using Olbrasoft.Travel.DataAccessLayer;
using Olbrasoft.Travel.DataAccessLayer.EntityFramework;

namespace Olbrasoft.Travel.Web.Mvc.Installers
{
    public class DataAccessLayerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<DbContext>().ImplementedBy<TravelContext>().LifestylePerWebRequest());
            container.Register(Component.For(typeof(IMappedEntitiesRepository<>)).ImplementedBy(typeof(MappedEntitiesRepository<>)).LifestylePerWebRequest());
        }
    }
}