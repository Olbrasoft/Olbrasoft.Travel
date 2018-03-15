using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class TravelContext : DbContext
    {
        public virtual IDbSet<User> Users { get; set; }
        public virtual IDbSet<LogOfImport> LogsOfImports { get; set; }
        public virtual IDbSet<Continent> Continents { get; set; }
        public virtual IDbSet<Language> Languages { get; set; }
        public virtual IDbSet<LocalizedContinent> LocalizedContinents { get; set; }
        public virtual IDbSet<Country> Countries { get; set; }
        public virtual IDbSet<LocalizedCountry> LocalizedCountries { get; set; }
        public virtual IDbSet<City> Cities { get; set; }
        public virtual IDbSet<LocalizedCity> LocalizedCities { get; set; }
        public virtual IDbSet<Neighborhood> Neighborhoods { get; set; }
        public virtual IDbSet<LocalizedNeighborhood> LocalizedNeighborhoods { get; set; }
        public virtual IDbSet<TypeOfRegion> TypesOfRegions { get; set; }
        public virtual IDbSet<SubClass> SubClasses { get; set; }
        public virtual IDbSet<Region> Regions { get; set; }
        public virtual IDbSet<LocalizedRegion> LocalizedRegions { get; set; }
        public virtual IDbSet<RegionToRegion> RegionsToRegions { get; set; }
        public virtual IDbSet<PointOfInterest> PointsOfInterest { get; set; }
        public virtual IDbSet<LocalizedPointOfInterest> LocalizedPointsOfInterest { get; set; }

        /// <summary>
        /// States and Provinces
        /// </summary>
        public virtual IDbSet<State> States { get; set; }

        /// <summary>
        /// Localized information about the State or Province in the current language
        /// </summary>
        public virtual IDbSet<LocalizedState> LocalizedStates { get; set; }



        //public virtual IDbSet<PointOfInterestToPointOfInterest> PointsOfInterestToPointsOfInterest { get; set; }
        //public virtual IDbSet<PointOfInterestToRegion> PointsOfInterestToRegions { get; set; }
        //public virtual IDbSet<PointOfInterestToSubClass> PointsOfInterestToSubClasses { get; set; }
        //public virtual IDbSet<Airport> Airports { get; set; }

        //public virtual IDbSet<SupportedCulture> SupportedCultures { get; set; }
        //public virtual IDbSet<Category> Categories { get; set; }
        //public virtual IDbSet<Accommodation> Accommodations { get; set; }
        //public virtual IDbSet<Chain> Chains { get; set; }
        //public virtual IDbSet<NameOfDescription> TypesOfDescriptions { get; set; }
        //public virtual IDbSet<Description> Descriptions { get; set; }
        //public virtual IDbSet<LocalizedCategory> LocalizedCategories { get; set; }
        //public virtual IDbSet<LocalizedAccommodation> LocalizedAccommodations { get; set; }


        public TravelContext() : base("name=Travel")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            OnUsersCreating(modelBuilder);
            OnLogsOfImportsCreating(modelBuilder);
            OnContinentsCreating(modelBuilder);
            OnLanguagesCreating(modelBuilder);
            OnLocalizedContinentsCreating(modelBuilder);
            OnCountriesCreating(modelBuilder);
            OnLocalizedCountriesCreating(modelBuilder);
            OnCitiesCreating(modelBuilder);
            OnLocalizedCitiesCreating(modelBuilder);
            OnNeighborhoodsCreating(modelBuilder);
            OnLocalizedNeighborhoodsCreating(modelBuilder);
            OnTypesOfRegionsCreating(modelBuilder);
            OnSubClassesCreating(modelBuilder);
            OnRegionsCreating(modelBuilder);
            OnLocalizedRegionsCreating(modelBuilder);
            OnRegionsToRegionsCreating(modelBuilder);
            OnPointsOfInterestCreating(modelBuilder);
            OnLocalizedPointsOfInterestCreating(modelBuilder);
            OnStatesCreating(modelBuilder);
            OnLocalizedStates(modelBuilder);

            //OnPointsOfInterestToPointsOfInterestCreating(modelBuilder);
            //OnPointsOfInterestToRegionsCreating(modelBuilder);

            //OnPointsOfInterestToSubClassesCreating(modelBuilder);
            //OnAirportsCreating(modelBuilder);

            //OnChainsCreating(modelBuilder);
            //OnCategoriesCreating(modelBuilder);
            //OnAccommodationsCreating(modelBuilder);
            //OnTypesOfDescriptionsCreating(modelBuilder);
            //OnLocalizedCategoriesCreating(modelBuilder);
            //OnDescriptionsCreating(modelBuilder);
            //OnSupportedCulturesCreating(modelBuilder);
            //OnLocalizedAccommodationsCreating(modelBuilder);

        }

        private void OnLocalizedStates(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LocalizedState>()
                .ToTable(nameof(LocalizedStates), "geo")
                .Property(e => e.DateAndTimeOfCreation)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);


            modelBuilder.Entity<LocalizedState>()
                .HasRequired(ls => ls.Language)
                .WithMany(l => l.LocalizedStates)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<LocalizedState>()
                .HasRequired(ls => ls.Creator)
                .WithMany(u => u.LocalizedStates)
                .WillCascadeOnDelete(false);
        }

        private void OnStatesCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<State>()
                .ToTable(nameof(States), "geo")
                .Property(e => e.DateAndTimeOfCreation)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            modelBuilder.Entity<State>()
                .HasRequired(state => state.Creator)
                .WithMany(user => user.States)
                .WillCascadeOnDelete(false);

        }


        //private void OnPointsOfInterestToSubClassesCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<PointOfInterestToSubClass>()
        //        .ToTable(nameof(PointsOfInterestToSubClasses), "geo")
        //        .Property(e => e.DateAndTimeOfCreation)
        //        .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

        //    modelBuilder.Entity<PointOfInterestToSubClass>()
        //        .HasRequired(pointOfInterestToSubClass
        //            => pointOfInterestToSubClass.PointOfInterest)
        //        .WithRequiredDependent(p => p.ToSubClass);

        //    modelBuilder.Entity<User>()
        //        .HasMany(user => user.CreatedPointsOfInterestToSubClasses)
        //        .WithRequired(pointOfInterestToSubClass => pointOfInterestToSubClass.Creator)
        //        .WillCascadeOnDelete(false);

        //}


        private void OnLocalizedNeighborhoodsCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LocalizedNeighborhood>()
                .ToTable(nameof(LocalizedNeighborhoods), "geo")
                .Property(e => e.DateAndTimeOfCreation)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            modelBuilder.Entity<LocalizedNeighborhood>()
                 .HasRequired(ln => ln.Language)
                 .WithMany(lan => lan.LocalizedNeighborhoods)
                 .WillCascadeOnDelete(false);

            modelBuilder.Entity<LocalizedNeighborhood>()
                .HasRequired(ln => ln.Creator)
                .WithMany(user => user.CreatedLocalizedNeighborhoods)
                .WillCascadeOnDelete(false);


        }

        private void OnNeighborhoodsCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Neighborhood>()
                .ToTable(nameof(Neighborhoods), "geo")
                .Property(e => e.DateAndTimeOfCreation)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            modelBuilder.Entity<Neighborhood>()
                .HasIndex(p => p.EanId)
                .IsUnique();
        }

        private void OnLocalizedCitiesCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LocalizedCity>()
                .ToTable(nameof(LocalizedCities), "geo")
                .Property(e => e.DateAndTimeOfCreation)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            modelBuilder.Entity<LocalizedCity>()
                .HasRequired(lc => lc.Language)
                .WithMany(lan => lan.LocalizedCities)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LocalizedCity>()
                .HasRequired(lc => lc.Creator)
                .WithMany(user => user.CreatedLocalizedCities)
                .WillCascadeOnDelete(false);
        }

        private void OnCitiesCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>()
                .ToTable(nameof(Cities), "geo")
                .Property(e => e.DateAndTimeOfCreation)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            modelBuilder.Entity<City>().HasRequired(c => c.Creator).WithMany(u => u.CreatedCities);

            modelBuilder.Entity<City>().HasIndex(p => p.EanId).IsUnique();
        }


        private void OnLocalizedCountriesCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LocalizedCountry>()
                .ToTable(nameof(LocalizedCountries), "geo");

            modelBuilder.Entity<LocalizedCountry>()
                .Property(e => e.DateAndTimeOfCreation)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            modelBuilder.Entity<LocalizedCountry>()
                .HasRequired(lc => lc.Language)
                .WithMany(lan => lan.LocalizedCountries).WillCascadeOnDelete(false);


            modelBuilder.Entity<LocalizedCountry>().HasRequired(lc => lc.Creator).WithMany(user => user.CreatedLocalizedCountries).WillCascadeOnDelete(false);
        }

        private void OnCountriesCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>()
                .ToTable(nameof(Countries), "geo")
                .HasIndex(p => p.EanId).IsUnique();
            
            modelBuilder.Entity<Country>()
                .HasIndex(p => p.Code).IsUnique();

            modelBuilder.Entity<Country>()
                .Property(e => e.DateAndTimeOfCreation)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            modelBuilder.Entity<Country>().HasRequired(c => c.Creator).WithMany(u => u.CreatedCountries).WillCascadeOnDelete(false);

        }

        private void OnLocalizedRegionsCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LocalizedRegion>()
                .ToTable(nameof(LocalizedRegions), "geo");

            modelBuilder.Entity<LocalizedRegion>()
                .Property(e => e.DateAndTimeOfCreation)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);


            modelBuilder.Entity<LocalizedRegion>().HasRequired(lr => lr.Language).WithMany(l => l.LocalizedRegions)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LocalizedRegion>().HasRequired(lr => lr.Creator)
                .WithMany(u => u.CreatedLocalizedRegions).WillCascadeOnDelete(false);

        }

        //private void OnPointsOfInterestToRegionsCreating(DbModelBuilder modelBuilder)
        //{

        //    // modelBuilder.Entity<Region>()
        //    //.HasMany(s => s.PointsOfInterest)
        //    //      .WithMany(c => c.Regions)
        //    //      .Map(cs =>
        //    //      {
        //    //          cs.MapLeftKey("ToId");
        //    //          cs.MapRightKey("Id");
        //    //          cs.ToTable("PointsOfInterestToRegions","geo");

        //    //      });

        //    modelBuilder.Entity<PointOfInterestToRegion>()
        //        .ToTable(nameof(PointsOfInterestToRegions), "geo");

        //    modelBuilder.Entity<PointOfInterestToRegion>()
        //        .HasRequired(pointOfInterestToRegion
        //            => pointOfInterestToRegion.PointOfInterest)
        //        .WithMany(pointOfInterest => pointOfInterest.PointsOfInterestToRegions)
        //        .HasForeignKey(pointOfInterestToRegion
        //            => pointOfInterestToRegion.Id)
        //        .WillCascadeOnDelete(false);

        //    modelBuilder.Entity<PointOfInterestToRegion>()
        //        .HasRequired(pointOfInterestToRegion
        //            => pointOfInterestToRegion.Region)
        //        .WithMany(region => region.PointsOfInterestToRegions)
        //        .HasForeignKey(pointOfInterestToRegion
        //            => pointOfInterestToRegion.ToId)
        //        .WillCascadeOnDelete(true);

        //    modelBuilder.Entity<User>()
        //        .HasMany(user => user.CreatedPointsOfInterestToRegions)
        //        .WithRequired(pointOfInterestToRegion => pointOfInterestToRegion.Creator)
        //        .WillCascadeOnDelete(false);

        //    modelBuilder.Entity<PointOfInterestToRegion>()
        //        .Property(pointOfInterestToRegion => pointOfInterestToRegion.DateAndTimeOfCreation)
        //        .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);


        //}

        //private void OnPointsOfInterestToPointsOfInterestCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<PointOfInterestToPointOfInterest>()
        //        .ToTable(nameof(PointsOfInterestToPointsOfInterest), "geo");

        //    modelBuilder.Entity<PointOfInterestToPointOfInterest>()
        //        .HasRequired(pointOfInterestToPointOfInterest
        //            => pointOfInterestToPointOfInterest.PointOfInterest)
        //        .WithMany(pointOfInterest => pointOfInterest.ToChildPointsOfInterest)
        //        .HasForeignKey(pointOfInterestToPointOfInterest
        //            => pointOfInterestToPointOfInterest.Id)
        //        .WillCascadeOnDelete(false);

        //    modelBuilder.Entity<PointOfInterestToPointOfInterest>()
        //        .HasRequired(pointOfInterestToPointOfInterest
        //            => pointOfInterestToPointOfInterest.ParentPointOfInterest)
        //        .WithMany(pointOfInterest => pointOfInterest.ToParentPointsOfInterest)
        //        .HasForeignKey(pointOfInterestToPointOfInterest
        //            => pointOfInterestToPointOfInterest.ToId)
        //        .WillCascadeOnDelete(true);

        //    modelBuilder.Entity<User>()
        //        .HasMany(user => user.CreatedPointsOfInterestToPointsOfInterest)
        //        .WithRequired(pointOfInterestToPointOfInterest => pointOfInterestToPointOfInterest.Creator)
        //        .WillCascadeOnDelete(false);

        //    modelBuilder.Entity<PointOfInterestToPointOfInterest>()
        //        .Property(pointOfInterestToPointOfInterest => pointOfInterestToPointOfInterest.DateAndTimeOfCreation)
        //        .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

        //}

        private void OnPointsOfInterestCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PointOfInterest>()
                .ToTable(nameof(PointsOfInterest), "geo")
                .HasIndex(pointOfInterest => pointOfInterest.EanId).IsUnique();

            modelBuilder.Entity<PointOfInterest>()
                .Property(e => e.DateAndTimeOfCreation)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
        }

        private void OnRegionsToRegionsCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RegionToRegion>()
                .ToTable(nameof(RegionsToRegions), "geo")
                .Property(e => e.DateAndTimeOfCreation)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            modelBuilder.Entity<RegionToRegion>()
                .HasRequired(regionToRegion => regionToRegion.Region)
                 .WithMany(region => region.ToChildRegions)
                .HasForeignKey(regionToRegion => regionToRegion.Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RegionToRegion>()
                .HasRequired(regionToRegion => regionToRegion.ParentRegion)
                .WithMany(region => region.ToParentRegions)
                .HasForeignKey(regionToRegion => regionToRegion.ToId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<RegionToRegion>().HasRequired(rtr => rtr.Creator)
                .WithMany(u => u.CreatedRegionsToRegions).WillCascadeOnDelete(false);

        }


        private void OnRegionsCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Region>()
                .ToTable(nameof(Regions), "geo")
                .HasIndex(p => p.EanId).IsUnique();

            modelBuilder.Entity<Region>()
                .Property(e => e.DateAndTimeOfCreation)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            modelBuilder.Entity<Region>().HasRequired(r=>r.Creator).WithMany(u=>u.CreatedRegions).WillCascadeOnDelete(false);
            
        }

        private static void OnTypesOfRegionsCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TypeOfRegion>()
                .ToTable(nameof(TypesOfRegions), "geo")
                .HasIndex(typeOfRegion => typeOfRegion.Name).IsUnique();

            modelBuilder.Entity<TypeOfRegion>()
                .Property(typeOfRegion => typeOfRegion.DateAndTimeOfCreation)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
        }

        private void OnSubClassesCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SubClass>()
                .ToTable(nameof(SubClasses), "geo")
                .HasIndex(p => p.Name).IsUnique();

            modelBuilder.Entity<SubClass>()
                .Property(e => e.DateAndTimeOfCreation)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
        }

        private void OnLocalizedContinentsCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LocalizedContinent>()
                .ToTable(nameof(LocalizedContinents), "geo");

            modelBuilder.Entity<LocalizedContinent>()
                .Property(e => e.DateAndTimeOfCreation)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            modelBuilder.Entity<LocalizedContinent>().HasRequired(lc => lc.Language)
                .WithMany(l => l.LocalizedContinents).WillCascadeOnDelete(false);

            modelBuilder.Entity<LocalizedContinent>().HasRequired(lc => lc.Creator)
                .WithMany(u => u.CreatedLocalizedContinents).WillCascadeOnDelete(false);
        }

        private void OnContinentsCreating(DbModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Continent>()
                .ToTable(nameof(Continents), "geo")
                .Property(continent => continent.DateAndTimeOfCreation)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            
        }

        private void OnLanguagesCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Language>()
                .ToTable(nameof(Languages))
                .Property(e => e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<Language>()
                .HasIndex(p => p.EanLanguageCode).IsUnique();

            modelBuilder.Entity<Language>()
                .Property(e => e.DateAndTimeOfCreation)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            modelBuilder.Entity<Language>().HasRequired(l => l.Creator).WithMany(u => u.CreatedLanguages);
        }

        //private void OnAirportsCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Airport>()
        //        .ToTable(nameof(Airports), "geo")
        //        .HasIndex(p => p.EanId)
        //        .IsUnique();

        //    modelBuilder.Entity<Airport>()
        //        .Property(e => e.DateAndTimeOfCreation)
        //        .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

        //    modelBuilder.Entity<User>()
        //        .HasMany(user => user.CreatedAirports)
        //        .WithRequired(airport => airport.Creator);
        //}


        private void OnLocalizedPointsOfInterestCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LocalizedPointOfInterest>()
                .ToTable(nameof(LocalizedPointsOfInterest), "geo")
                .Property(e => e.DateAndTimeOfCreation)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            modelBuilder.Entity<LocalizedPointOfInterest>().HasRequired(lpoi => lpoi.Language)
                .WithMany(l => l.LocalizedPointsOfInterest).WillCascadeOnDelete(false);

            modelBuilder.Entity<LocalizedPointOfInterest>().HasRequired(localizedPointOfInterest => localizedPointOfInterest.Creator)
                .WithMany(user => user.CreatedLocalizedPointsOfInterest).WillCascadeOnDelete(false);


        }


        private void OnLogsOfImportsCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LogOfImport>()
                .ToTable("LogsOfImports")
                .Property(e => e.DateAndTimeOfCreation)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
        }


        private void OnUsersCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(p => p.UserName).IsUnique();
            modelBuilder.Entity<User>().Property(e => e.DateAndTimeOfCreation)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
        }





        private void OnLocalizedAccommodationsCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LocalizedAccommodation>()
                .ToTable("acco.LocalizedAccommodations");
        }

        private void OnLocalizedCategoriesCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LocalizedCategory>()
                .ToTable("acco.LocalizedCategories");

            modelBuilder.Entity<SupportedCulture>()
                .HasMany(e => e.LocalizedCategories)
                .WithRequired(e => e.SupportedCulture)
                .WillCascadeOnDelete(true);
        }

        private void OnSupportedCulturesCreating(DbModelBuilder modelBuilder)
        {
            //todo not reflection Migration changing manual 
            //Id = c.Int(nullable: false, identity: false)
            modelBuilder.Entity<SupportedCulture>()
               .HasKey(e => e.Id)
               .Property(e => e.Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }

        private void OnTypesOfDescriptionsCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<NameOfDescription>()
                .ToTable("acco.TypesOfDescriptions")
                .HasIndex(p => p.Name).IsUnique();
        }


        private void OnAccommodationsCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Accommodation>()
                .ToTable("acco.Accommodations")
                .HasIndex(e => e.EanId).IsUnique();


            modelBuilder.Entity<Chain>()
                .HasMany(e => e.Accommodations)
                .WithRequired(e => e.Chain)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Category>()
            .HasMany(e => e.Accommodations)
                .WithRequired(e => e.Category)
                .WillCascadeOnDelete(true);

        }

        private void OnDescriptionsCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Description>().ToTable("Descriptions", "acco");

            //modelBuilder.Entity<Accommodation>()
            //    .HasMany(e => e.Descriptions)
            //    .WithRequired(e => e.Accommodation)
            //    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<NameOfDescription>()
            //    .ToTable("acco.TypesOfDescriptions")
            //    .HasMany(e => e.Descriptions)
            //    .WithRequired(e => e.NameOfDescription)
            //    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<SupportedCulture>()
            //    .HasMany(e => e.Descriptions)
            //    .WithRequired(e => e.SupportedCulture)
            //    .WillCascadeOnDelete(true);

        }

        private static void OnChainsCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chain>()
                .ToTable("acco.Chains").HasIndex(p => p.EanId).IsUnique();

        }

        private static void OnCategoriesCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .ToTable("acco.Categories");


            modelBuilder.Entity<Category>()
                .HasIndex(p => p.EanId).IsUnique();

        }


    }

}
