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
        public virtual IDbSet<SubClass> SubClasses { get; set; }
        public virtual IDbSet<Region> Regions { get; set; }
        public virtual IDbSet<PointOfInterest> PointsOfInterest { get; set; }
        public virtual IDbSet<Language> Languages { get; set; }
        public virtual IDbSet<LocalizedRegion> LocalizedRegions { get; set; }
        public virtual IDbSet<LocalizedPointOfInterest> LocalizedPointsOfInterest { get; set; }
        public virtual IDbSet<RegionToRegion> RegionsToRegions { get; set; }
        public virtual IDbSet<PointOfInterestToPointOfInterest> PointsOfInterestToPointsOfInterest { get; set; }
        public  virtual IDbSet<PointOfInterestToRegion> PointsOfInterestToRegions { get; set; }


        //public virtual IDbSet<SupportedCulture> SupportedCultures { get; set; }
        //public virtual IDbSet<Category> Categories { get; set; }
        //public virtual IDbSet<Accommodation> Accommodations { get; set; }
        //public virtual IDbSet<Chain> Chains { get; set; }
        //public virtual IDbSet<TypeOfDescription> TypesOfDescriptions { get; set; }
        //public virtual IDbSet<Description> Descriptions { get; set; }
        //public virtual IDbSet<LocalizedCategory> LocalizedCategories { get; set; }
        //public virtual IDbSet<LocalizedAccommodation> LocalizedAccommodations { get; set; }



        //public virtual IDbSet<Country> Countries { get; set; }


        public TravelContext() : base("name=Travel")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            OnUsersCreating(modelBuilder);
            OnLogsOfImportsCreating(modelBuilder);
            OnTypesOfRegionsCreating(modelBuilder);
            OnSubClassesCreating(modelBuilder);
            OnRegionsCreating(modelBuilder);
            OnPointsOfInterestCreating(modelBuilder);
            OnLanguagesCreating(modelBuilder);
            OnLocalizedRegionsCreating(modelBuilder);
            OnLocalizedPointsOfInterestCreating(modelBuilder);
            OnRegionsToRegionsCreating(modelBuilder);
            OnPointsOfInterestToPointsOfInterestCreating(modelBuilder);
            OnPointsOfInterestToRegionCreating(modelBuilder);
            //OnChainsCreating(modelBuilder);
            //OnCategoriesCreating(modelBuilder);
            //OnAccommodationsCreating(modelBuilder);
            //OnTypesOfDescriptionsCreating(modelBuilder);
            //OnLocalizedCategoriesCreating(modelBuilder);
            //OnDescriptionsCreating(modelBuilder);
            //OnSupportedCulturesCreating(modelBuilder);
            //OnLocalizedAccommodationsCreating(modelBuilder);

            //OnCountriesCreating(modelBuilder);

        }

        private void OnPointsOfInterestToRegionCreating(DbModelBuilder modelBuilder)
        {

            // modelBuilder.Entity<Region>()
            //.HasMany(s => s.PointsOfInterest)
            //      .WithMany(c => c.Regions)
            //      .Map(cs =>
            //      {
            //          cs.MapLeftKey("RegionId");
            //          cs.MapRightKey("PointOfInterestId");
            //          cs.ToTable("PointsOfInterestToRegions","geo");

            //      });

            modelBuilder.Entity<PointOfInterestToRegion>()
                .ToTable("PointsOfInterestToRegions", "geo");

            modelBuilder.Entity<PointOfInterestToRegion>()
                .HasRequired(pointOfInterestToRegion
                    => pointOfInterestToRegion.PointOfInterest)
                .WithMany(pointOfInterest => pointOfInterest.PointsOfInterestToRegions)
                .HasForeignKey(pointOfInterestToRegion
                    => pointOfInterestToRegion.PointOfInterestId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PointOfInterestToRegion>()
                .HasRequired(pointOfInterestToRegion
                    => pointOfInterestToRegion.Region)
                .WithMany(region => region.PointsOfInterestToRegions)
                .HasForeignKey(pointOfInterestToRegion
                    => pointOfInterestToRegion.RegionId)
                .WillCascadeOnDelete(false);

        }

        private void OnPointsOfInterestToPointsOfInterestCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PointOfInterestToPointOfInterest>()
                .ToTable("PointsOfInterestToPointsOfInterest", "geo");

            modelBuilder.Entity<PointOfInterestToPointOfInterest>()
                .HasRequired(pointOfInterestToPointOfInterest 
                    => pointOfInterestToPointOfInterest.PointOfInterest)
                .WithMany(pointOfInterest => pointOfInterest.ToChildPointsOfInterest)
                .HasForeignKey(pointOfInterestToPointOfInterest 
                    => pointOfInterestToPointOfInterest.PointOfInterestId)
                .WillCascadeOnDelete(true);
            
            modelBuilder.Entity<PointOfInterestToPointOfInterest>()
                .HasRequired(pointOfInterestToPointOfInterest 
                    => pointOfInterestToPointOfInterest.ParentPointOfInterest)
                .WithMany(pointOfInterest => pointOfInterest.ToParentPointsOfInterest)
                .HasForeignKey(pointOfInterestToPointOfInterest 
                    => pointOfInterestToPointOfInterest.ParentPointOfInterestId)
                .WillCascadeOnDelete(false);
        }

        private void OnRegionsToRegionsCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RegionToRegion>()
                .ToTable("RegionsToRegions", "geo");

            modelBuilder.Entity<RegionToRegion>()
                .HasRequired(regionToRegion => regionToRegion.Region)
                .WithMany(region => region.ToChildRegions)
                .HasForeignKey(regionToRegion => regionToRegion.RegionId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<RegionToRegion>()
                .HasRequired(regionToRegion => regionToRegion.ParentRegion)
                .WithMany(region => region.ToParentRegions)
                .HasForeignKey(regionToRegion => regionToRegion.ParentRegionId)
                .WillCascadeOnDelete(false);
        }

        private void OnLocalizedPointsOfInterestCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LocalizedPointOfInterest>()
                .ToTable("LocalizedPointsOfInterest", "geo");

            modelBuilder.Entity<LocalizedPointOfInterest>()
                .Property(e => e.DateAndTimeOfCreation)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            modelBuilder.Entity<Language>()
                .HasMany(language => language.LocalizedPointsOfInterest)
                .WithRequired(localizedPointsOfInterest => localizedPointsOfInterest.Language)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(user => user.LocalizedPointsOfInterest)
                .WithRequired(localizedPointsOfInterest => localizedPointsOfInterest.Creator)
                .WillCascadeOnDelete(false);
        }

        private void OnLocalizedRegionsCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LocalizedRegion>()
                .ToTable("LocalizedRegions", "geo");

            modelBuilder.Entity<LocalizedRegion>()
                .Property(e => e.DateAndTimeOfCreation)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            modelBuilder.Entity<Language>()
                .HasMany(language => language.LocalizedRegions)
                .WithRequired(localizedRegion => localizedRegion.Language)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(user => user.LocalizedRegions)
                .WithRequired(localizedRegion => localizedRegion.Creator)
                .WillCascadeOnDelete(false);

        }

        private void OnLanguagesCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Language>()
                .ToTable("Languages")
                .Property(e => e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<Language>()
                .HasIndex(p => p.EanLanguageCode).IsUnique();

            modelBuilder.Entity<Language>()
                .Property(e => e.DateAndTimeOfCreation)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
        }

        private void OnPointsOfInterestCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PointOfInterest>()
                .ToTable("PointsOfInterest", "geo")
                .HasIndex(p => p.EanRegionId).IsUnique();

            modelBuilder.Entity<PointOfInterest>()
                .Property(e => e.DateAndTimeOfCreation)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
        }


        private void OnRegionsCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Region>()
                .ToTable("Regions", "geo")
                .HasIndex(p => p.EanRegionId).IsUnique();

            modelBuilder.Entity<Region>()
                .Property(e => e.DateAndTimeOfCreation)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            //AddColumnCoordinatesToRegions
            //modelBuilder.Entity<Region>()
            //    .Property(t => t.Coordinates)
            //    .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_Regions_coordinates")));
        }

        private void OnSubClassesCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SubClass>()
                .ToTable("SubClasses", "geo")
                .HasIndex(p => p.Name).IsUnique();

            modelBuilder.Entity<SubClass>()
                .Property(e => e.DateAndTimeOfCreation)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
        }


        private static void OnTypesOfRegionsCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TypeOfRegion>()
                .ToTable("TypesOfRegions", "geo")
                .HasIndex(p => p.Name).IsUnique();

            modelBuilder.Entity<TypeOfRegion>()
                .Property(e => e.DateAndTimeOfCreation)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
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

        private void OnCountriesCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>()
                .ToTable("Countries", "geo");

            modelBuilder.Entity<Country>()
                 .Property(e => e.RegionId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<Country>()
                .HasIndex(p => p.Code).IsUnique();
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

            modelBuilder.Entity<TypeOfDescription>()
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

            //modelBuilder.Entity<TypeOfDescription>()
            //    .ToTable("acco.TypesOfDescriptions")
            //    .HasMany(e => e.Descriptions)
            //    .WithRequired(e => e.TypeOfDescription)
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
