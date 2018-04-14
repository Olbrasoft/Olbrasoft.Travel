using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework.Migrations
{
    public class TravelMigrationsConfiguration : DbMigrationsConfiguration<TravelContext>
    {
        public TravelMigrationsConfiguration()
        {
            AutomaticMigrationsEnabled = false;
            SetSqlGenerator("System.Data.SqlClient", new CustomSqlServerMigrationSqlGenerator());
        }

        protected override void Seed(TravelContext context)
        {

            var travelUser = new User
            {
                UserName = "Travel"
            };

            var storedTravelUser = context.Set<User>().FirstOrDefault(u => u.UserName == travelUser.UserName);

            if (storedTravelUser == null)
            {
                context.Set<User>().AddOrUpdate(travelUser);
            }
            else
            {
                travelUser = storedTravelUser;
            }

            var storedTypesOfRegionsNames = new HashSet<string>(context.Set<TypeOfRegion>().Select(t => t.Name));

            var regionNames = new HashSet<string>
            {
                    "World",
                    "Continent",
                    "Country",
                    "Province (State)",
                    "Multi-Region (within a country)",
                    "Multi-City (Vicinity)",
                    "City",
                    "Neighborhood",
                    "Point of Interest",
                    "Point of Interest Shadow",
                    "Airport",
                    "Train Station"
            };

            foreach (var regionName in regionNames)
            {
                if (!storedTypesOfRegionsNames.Contains(regionName))
                {
                    context.Set<TypeOfRegion>().Add(new TypeOfRegion { Name = regionName, Creator = travelUser });
                }

            }
            context.SaveChanges();

            var storedTypesOfAttributesNames = new HashSet<string>(context.Set<TypeOfAttribute>().Select(t => t.Name));

            var typesOfAttributesNames = new HashSet<string>
            {
                "Amenity",
                "Policy"
            };

            foreach (var typeOfAttributeName in typesOfAttributesNames)
            {
                if (!storedTypesOfAttributesNames.Contains(typeOfAttributeName))
                {
                    context.Set<TypeOfAttribute>().Add(new TypeOfAttribute { Name = typeOfAttributeName, Creator = travelUser });
                }
            }
            context.SaveChanges();


            var storedSubTypesOfAttributesNames = new HashSet<string>(context.Set<SubTypeOfAttribute>().Select(s=>s.Name));

            var subTypesOfAttributesNames = new HashSet<string>
            {
                "AmenityOfAccommodation",
                "AmenityOfRoom",
                "CheckInOut",
                "Other",
                "Payment",
                "Pets"
            };

            foreach (var subTypesOfAttributesName in subTypesOfAttributesNames)
            {
                if (!storedSubTypesOfAttributesNames.Contains(subTypesOfAttributesName))
                {
                    context.Set<SubTypeOfAttribute>().Add(new SubTypeOfAttribute(){Name = subTypesOfAttributesName, Creator = travelUser});
                }
            }
            context.SaveChanges();

        }

    }
}