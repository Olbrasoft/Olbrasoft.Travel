using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.DAL.EntityFramework
{
    public class TravelDbContext : DbContext
    {

        public virtual IDbSet<SupportedCulture> SupportedCultures { get; set; }
        public virtual IDbSet<Category> Categories { get; set; }
        public virtual IDbSet<Accommodation> Accommodations { get; set; }
        public virtual IDbSet<Chain> Chains { get; set; }
        public virtual IDbSet<TypeOfDescription> TypesOfDescriptions { get; set; }
        public virtual IDbSet<Description> Descriptions { get; set; }
        public virtual IDbSet<LocalizedCategory> LocalizedCategories { get; set; }
        public virtual IDbSet<LocalizedAccommodation> LocalizedAccommodations { get; set; }
        public virtual IDbSet<TypeOfRegion> TypesOfRegions { get; set; }
        public virtual IDbSet<SubClassOfRegion> SubClassesOfRegions { get; set; }
        public virtual IDbSet<Region> Regions { get; set; }
        public virtual IDbSet<LocalizedRegion> LocalizedRegions { get; set; }
        public virtual IDbSet<RegionToRegion> RegionsToRegions { get; set; }
        public virtual IDbSet<Country> Countries { get; set; }


        public TravelDbContext() : base("name=Travel")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {         

            OnChainsCreating(modelBuilder);
            OnCategoriesCreating(modelBuilder);
            OnAccommodationsCreating(modelBuilder);
            OnTypesOfDescriptionsCreating(modelBuilder);
            OnLocalizedCategoriesCreating(modelBuilder);
            OnDescriptionsCreating(modelBuilder);
            OnSupportedCulturesCreating(modelBuilder);
            OnLocalizedAccommodationsCreating(modelBuilder);
            OnTypesOfRegionsCreating(modelBuilder);
            OnSubClassesOfRegionsCreating(modelBuilder);
            OnRegionsCreating(modelBuilder);
            OnLocalizedRegionsCreating(modelBuilder);
            OnRegionsToRegionsCreating(modelBuilder);
            OnCountriesCreating(modelBuilder);

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

        private void OnRegionsToRegionsCreating(DbModelBuilder modelBuilder)
        {         
            modelBuilder.Entity<RegionToRegion>()
                .ToTable("RegionsToRegions","geo");
            
            modelBuilder.Entity<RegionToRegion>()
                .HasRequired(i => i.Region)
                .WithMany(i => i.ToChildRegions)
                .HasForeignKey(i => i.RegionId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<RegionToRegion>()
                .HasRequired(i => i.ParentRegion)
                .WithMany(i => i.ToParentRegions)
                .HasForeignKey(i => i.ParentRegionId)
                .WillCascadeOnDelete(false);
        }

        private void OnLocalizedRegionsCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LocalizedRegion>()
                .ToTable("LocalizedRegions","geo");
                
        }

        private void OnRegionsCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Region>()
                .ToTable("Regions","geo")
                .HasIndex(p=>p.EanRegionId).IsUnique();

            //AddColumnCoordinatesToRegions
            //modelBuilder.Entity<Region>()
            //    .Property(t => t.Coordinates)
            //    .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_Regions_coordinates")));
        }

        private void OnSubClassesOfRegionsCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SubClassOfRegion>()
                .ToTable("SubClassesOfRegions","geo")
                .HasIndex(p => p.Name).IsUnique();
        }


        private void OnTypesOfRegionsCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TypeOfRegion>()
                .ToTable("TypesOfRegions","geo")
                .HasIndex(p => p.Name).IsUnique();
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
               .HasKey(e=>e.Id)
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
                .HasIndex(e=>e.EanId).IsUnique();


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
            modelBuilder.Entity<Description>().ToTable("Descriptions","acco");

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
