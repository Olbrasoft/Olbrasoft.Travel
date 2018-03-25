using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class TravelContext : DbContext
    {
        public virtual IDbSet<User> Users { get; set; }
        public virtual IDbSet<LogOfImport> LogsOfImports { get; set; }
        public virtual IDbSet<TypeOfRegion> TypesOfRegions { get; set; }
        public virtual IDbSet<Region> Regions { get; set; }

        public virtual IDbSet<SubClass> SubClasses { get; set; }

        public virtual IDbSet<Language> Languages { get; set; }

        public virtual IDbSet<RegionToType> RegionsToTypes { get; set; }

        public virtual IDbSet<LocalizedRegion> LocalizedRegions { get; set; }

        public virtual IDbSet<RegionToRegion> RegionsToRegions { get; set; }

        public virtual IDbSet<Country> Countries { get; set; }

        public virtual IDbSet<Airport> Airports { get; set; }

        //public virtual IDbSet<Travel.EAN.DTO.Geography.RegionCenter> RegionsCenters { get; set; }

        //public virtual IDbSet<NotFoundCountry> NotFoundCountries { get; set; }

        //public virtual IDbSet<DupNeighborhood> Neighborhoods { get; set; }   

        //public virtual IDbSet<DupCity> Cities { get; set; }

        //public virtual IDbSet<DupPoint> Points { get; set; }

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

            //modelBuilder.Entity<EAN.DTO.Geography.RegionCenter>().Property(p => p.RegionID)
            //    .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            OnUsersCreating(modelBuilder);
            OnLogsOfImportsCreating(modelBuilder);
            OnTypesOfRegionsCreating(modelBuilder);
            OnRegionsCreating(modelBuilder);
            OnSubClassesCreating(modelBuilder);
            OnLanguagesCreating(modelBuilder);
            OnRegionsToTypes(modelBuilder);
            OnLocalizedRegions(modelBuilder);

            OnRegionsToRegionsCreating(modelBuilder);
            OnCountriesCreating(modelBuilder);
            OnAirportsCreating(modelBuilder);

            //OnNeighborhoodsCreating(modelBuilder);
            //OnCitiesCreating(modelBuilder);
            //OnPointsCreating(modelBuilder);

            //OnPointsOfInterestToPointsOfInterestCreating(modelBuilder);
            //OnPointsOfInterestToRegionsCreating(modelBuilder);
            //OnPointsOfInterestToSubClassesCreating(modelBuilder);

            //OnChainsCreating(modelBuilder);
            //OnCategoriesCreating(modelBuilder);
            //OnAccommodationsCreating(modelBuilder);
            //OnTypesOfDescriptionsCreating(modelBuilder);
            //OnLocalizedCategoriesCreating(modelBuilder);
            //OnDescriptionsCreating(modelBuilder);
            //OnSupportedCulturesCreating(modelBuilder);
            //OnLocalizedAccommodationsCreating(modelBuilder);

        }

        private void OnRegionsToTypes(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RegionToType>()
                .ToTable(nameof(RegionsToTypes), "geo")
                .Property(e => e.DateAndTimeOfCreation)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            modelBuilder.Entity<RegionToType>().HasRequired(rtp => rtp.Creator).WithMany(user => user.RegionsToTypes)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RegionToType>().HasRequired(rtp => rtp.Region).WithMany(region => region.RegionsToTypes)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<RegionToType>().HasRequired(rtt => rtt.TypeOfRegion).WithMany(tor => tor.RegionsToTypes)
                .HasForeignKey(p => p.ToId).WillCascadeOnDelete(false);


        }

        //private void OnPointsCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<DupPoint>()
        //        .ToTable(nameof(Points),"geo")
        //        .Property(p => p.RegionId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        //}

        //private void OnCitiesCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<DupCity>().ToTable(nameof(Cities), "geo")
        //        .Property(c => c.RegionId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

        //}

        //private void OnNeighborhoodsCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<DupNeighborhood>()
        //        .ToTable(nameof(Neighborhoods), "geo")
        //        .Property(n => n.RegionId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        //}

        private void OnAirportsCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Airport>()
                .ToTable(nameof(Airports), "geo")
                .Property(c => c.DateAndTimeOfCreation)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            modelBuilder.Entity<Airport>().HasIndex(c => c.Id).IsUnique();
            modelBuilder.Entity<Airport>().HasIndex(c => c.Code).IsUnique();
            modelBuilder.Entity<Airport>().HasRequired(c => c.Region).WithOptional(r => r.AdditionalAirportProperties).WillCascadeOnDelete(true);
            modelBuilder.Entity<Airport>().HasRequired(c => c.Creator).WithMany(user => user.Airports).WillCascadeOnDelete(false);
        }


        private void OnCountriesCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Country>()
                .ToTable(nameof(Countries), "geo").Property(c => c.DateAndTimeOfCreation)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            modelBuilder.Entity<Country>().HasIndex(c => c.Id).IsUnique();
            modelBuilder.Entity<Country>().HasIndex(c => c.Code).IsUnique();


            modelBuilder.Entity<Country>().HasRequired(c => c.Region).WithOptional(r => r.AdditionalCountryProperties).WillCascadeOnDelete(true);

            modelBuilder.Entity<Country>().HasRequired(c => c.Creator).WithMany(user => user.Countries).WillCascadeOnDelete(false);

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
                .WithMany(u => u.RegionsToRegions).WillCascadeOnDelete(false);

        }

        //private void OnRegionsToSubClasses(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<RegionToSubClass>()
        //        .ToTable(nameof(RegionsToSubClasses), "geo")
        //        .Property(e => e.DateAndTimeOfCreation)
        //        .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

        //    modelBuilder.Entity<RegionToSubClass>()
        //        .HasRequired(rts => rts.Creator).WithMany(user => user.RegionsToSubClasses).WillCascadeOnDelete(false);

        //    modelBuilder.Entity<RegionToSubClass>()
        //        .HasRequired(rts => rts.Region).WithRequiredDependent(r => r.ToSubClass);

        //    modelBuilder.Entity<RegionToSubClass>().HasRequired(rts => rts.SubClass)
        //        .WithMany(sc => sc.RegionsToSubClasses);

        //}

        private void OnLocalizedRegions(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LocalizedRegion>()
                .ToTable(nameof(LocalizedRegions), "geo")
                .Property(e => e.DateAndTimeOfCreation)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            modelBuilder.Entity<LocalizedRegion>()
                .HasRequired(lr => lr.Creator)
                .WithMany(user => user.LocalizedRegions)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LocalizedRegion>()
                .HasRequired(lr => lr.Language)
                .WithMany(l => l.LocalizedRegions)
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
        //        .WithRequiredDependent(p => p.OneToMany);

        //    modelBuilder.Entity<User>()
        //        .HasMany(user => user.CreatedPointsOfInterestToSubClasses)
        //        .WithRequired(pointOfInterestToSubClass => pointOfInterestToSubClass.Creator)
        //        .WillCascadeOnDelete(false);

        //}


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

        private void OnRegionsCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Region>()
                .ToTable(nameof(Regions), "geo")
                .HasIndex(p => p.EanId).IsUnique();


            modelBuilder.Entity<Region>()
                .Property(e => e.DateAndTimeOfCreation)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            modelBuilder.Entity<Region>().HasRequired(r => r.Creator).WithMany(u => u.Regions).WillCascadeOnDelete(false);

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

            modelBuilder.Entity<Language>().HasRequired(l => l.Creator).WithMany(u => u.Languages);
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
