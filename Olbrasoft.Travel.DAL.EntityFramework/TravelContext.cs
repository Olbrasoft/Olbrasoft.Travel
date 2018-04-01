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
        public virtual IDbSet<TypeOfAccommodation> TypesOfAccommodations { get; set; }
        public virtual IDbSet<LocalizedTypeOfAccommodation> LocalizedTypesOfAccommodations { get; set; }
        public virtual IDbSet<Chain> Chains { get; set; }
        public virtual IDbSet<Accommodation> Accommodations { get; set; }
        public virtual IDbSet<LocalizedAccommodation> LocalizedAccommodations { get; set; }
        public virtual IDbSet<TypeOfDescription> TypesOfDescriptions { get; set; }
        public virtual IDbSet<Description> Descriptions { get; set; }
        public virtual IDbSet<PathToPhoto> PathsToPhotos { get; set; }
        public virtual IDbSet<FileExtension> FilesExtensions { get; set; }
        public virtual IDbSet<Caption> Captions { get; set; }
        public virtual IDbSet<LocalizedCaption> LocalizedCaptions { get; set; }
        public virtual IDbSet<PhotoOfAccommodation> PhotosOfAccommodations { get; set; }
        public virtual IDbSet<TypeOfRoom> TypesOfRooms { get; set; }

        public virtual IDbSet<LocalizedTypeOfRoom> LocalizedTypesOfRooms { get; set; }


        public TravelContext() : base("name=Travel")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
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
            
            OnTypesOfAccommodationsCreating(modelBuilder);
            OnLocalizedTypesOfAccommodationsCreating(modelBuilder);
            OnChainsCreating(modelBuilder);
            OnAccommodationsCreating(modelBuilder);
            OnLocalizedAccommodationsCreating(modelBuilder);
            OnTypesOfDescriptionsCreating(modelBuilder);
            OnDescriptionsCreating(modelBuilder);

            OnPathsToPhotosCreating(modelBuilder);
            OnFilesExtensionsCreating(modelBuilder);

            OnCaptionsCreating(modelBuilder);
            OnLocalizedCaptions(modelBuilder);

            OnPhotosOfAccommodationsCreating(modelBuilder);

            OnTypesOfRoomsCreating(modelBuilder);
            OnLocalizedTypesOfRoomsCreating(modelBuilder);

        }


        private void OnLocalizedTypesOfRoomsCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LocalizedTypeOfRoom>()
                .ToTable(nameof(LocalizedTypesOfRooms), "acco").HasRequired(ltor => ltor.Creator)
                .WithMany(u => u.LocalizedTypesOfRooms).WillCascadeOnDelete(false);

            modelBuilder.Entity<LocalizedTypeOfRoom>()
                .HasRequired(ltor => ltor.Language).WithMany(l => l.LocalizedTypesOfRooms).WillCascadeOnDelete(false);
        }


        private void OnLocalizedCaptions(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LocalizedCaption>()
                .HasRequired(lc => lc.Language).WithMany(l => l.LocalizedCaptions).WillCascadeOnDelete(false);

            modelBuilder.Entity<LocalizedCaption>().HasRequired(lc => lc.Creator).WithMany(u => u.LocalizedCaptions)
                .WillCascadeOnDelete(false);

        }

        private void OnTypesOfRoomsCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TypeOfRoom>().ToTable(nameof(TypesOfRooms), "acco")
                .HasRequired(tor => tor.Creator)
                .WithMany(u => u.TypesOfRooms)
                .WillCascadeOnDelete(false)
                ;

            modelBuilder.Entity<TypeOfRoom>().HasRequired(tor => tor.Accommodation)
                .WithMany(a => a.TypesOfRooms).WillCascadeOnDelete();

            modelBuilder.Entity<TypeOfRoom>().HasMany(tor => tor.PhotosOfAccommodations)
                .WithOptional(poa => poa.TypeOfRoom);
        }


        private void OnCaptionsCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Caption>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }

        private void OnFilesExtensionsCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FileExtension>().ToTable(nameof(FilesExtensions)).HasIndex(fe => fe.Extension)
                .IsUnique();

        }

        private void OnPathsToPhotosCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PathToPhoto>().ToTable(nameof(PathsToPhotos))
                .HasIndex(pathToPhoto => pathToPhoto.Path).IsUnique();

        }

        private void OnPhotosOfAccommodationsCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PhotoOfAccommodation>().ToTable(nameof(PhotosOfAccommodations), "acco")
               .HasIndex(p => new { p.PathToPhotoId, p.FileName }).IsUnique();

            modelBuilder.Entity<PhotoOfAccommodation>().HasRequired(p => p.Accommodation).WithMany(p => p.PhotosOfAccommodations)
                .HasForeignKey(p => p.AccommodationId).WillCascadeOnDelete(true);

            modelBuilder.Entity<PhotoOfAccommodation>().HasRequired(poa => poa.PathToPhoto)
                .WithMany(ptp => ptp.PhotosOfAccommodations).WillCascadeOnDelete(false);

            modelBuilder.Entity<PhotoOfAccommodation>().HasRequired(poa => poa.FileExtension)
                .WithMany(fe => fe.PhotosOfAccommodations).WillCascadeOnDelete(false);

            modelBuilder.Entity<PhotoOfAccommodation>().HasRequired(poa => poa.Creator)
                .WithMany(u => u.PhotosOfAccommodations).WillCascadeOnDelete(false);
        }

        private void OnDescriptionsCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Description>().ToTable(nameof(Descriptions), "acco")
                .Property(d => d.DateAndTimeOfCreation).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            modelBuilder.Entity<Description>().HasRequired(d => d.Accommodation).WithMany(a => a.Descriptions).WillCascadeOnDelete(true);

            modelBuilder.Entity<Description>().HasRequired(d => d.TypeOfDescription).WithMany(tod => tod.Descriptions).WillCascadeOnDelete(false);

            modelBuilder.Entity<Description>().HasRequired(d => d.Language).WithMany(l => l.Descriptions).WillCascadeOnDelete(false);

            modelBuilder.Entity<Description>().HasRequired(d => d.Creator).WithMany(u => u.Descriptions).WillCascadeOnDelete(false);
        }


        private void OnLocalizedAccommodationsCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LocalizedAccommodation>()
                .ToTable(nameof(LocalizedAccommodations), "acco")
                .Property(la => la.DateAndTimeOfCreation)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            modelBuilder.Entity<LocalizedAccommodation>().HasRequired(la => la.Creator)
                .WithMany(user => user.LocalizedAccommodations).WillCascadeOnDelete(false);

            modelBuilder.Entity<LocalizedAccommodation>().HasRequired(la => la.Accommodation)
                .WithMany(a => a.LocalizedAccommodations).WillCascadeOnDelete(true);

            modelBuilder.Entity<LocalizedAccommodation>().HasRequired(la => la.Language)
                .WithMany(l => l.LocalizedAccommodations).WillCascadeOnDelete(false);


        }
        

        private static void OnChainsCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chain>()
                .ToTable(nameof(Chains), "acco").HasIndex(p => p.EanId).IsUnique();

            modelBuilder.Entity<Chain>().Property(ch => ch.DateAndTimeOfCreation)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            modelBuilder.Entity<Chain>().HasRequired(ch => ch.Creator).WithMany(user => user.Chains)
                .WillCascadeOnDelete(true);

        }
        
        private void OnLocalizedTypesOfAccommodationsCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LocalizedTypeOfAccommodation>().ToTable(nameof(LocalizedTypesOfAccommodations), "acco")
                .Property(ltoa => ltoa.DateAndTimeOfCreation)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            modelBuilder.Entity<LocalizedTypeOfAccommodation>().HasRequired(ltoa => ltoa.Creator)
                .WithMany(user => user.LocalizedTypesOfAccommodations).WillCascadeOnDelete(false);

            modelBuilder.Entity<LocalizedTypeOfAccommodation>().HasRequired(ltoa => ltoa.TypeOfAccommodation)
                .WithMany(toa => toa.LocalizedTypesOfAccommodations).WillCascadeOnDelete(true);

            modelBuilder.Entity<LocalizedTypeOfAccommodation>().HasRequired(ltoa => ltoa.Language)
                .WithMany(l => l.LocalizedTypesOfAccommodations).WillCascadeOnDelete(false);
        }

        private void OnTypesOfAccommodationsCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TypeOfAccommodation>()
                .ToTable(nameof(TypesOfAccommodations), "acco")
                .Property(e => e.DateAndTimeOfCreation)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            modelBuilder.Entity<TypeOfAccommodation>().HasIndex(toa => toa.EanId).IsUnique();

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
                .ToTable(nameof(LogsOfImports))
                .Property(e => e.DateAndTimeOfCreation)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
        }


        private void OnUsersCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable(nameof(Users))
                .HasIndex(p => p.UserName).IsUnique();

            modelBuilder.Entity<User>().Property(e => e.DateAndTimeOfCreation)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
        }


        private void OnTypesOfDescriptionsCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TypeOfDescription>()
                .ToTable(nameof(TypesOfDescriptions), "acco")
                .HasIndex(p => p.Name).IsUnique();

            modelBuilder.Entity<TypeOfDescription>().Property(e => e.DateAndTimeOfCreation)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            modelBuilder.Entity<TypeOfDescription>().HasRequired(tod => tod.Creator)
                .WithMany(user => user.TypesOfDescriptions).WillCascadeOnDelete(true);
        }


        private void OnAccommodationsCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Accommodation>()
                .ToTable(nameof(Accommodations), "acco")
                .HasIndex(e => e.EanId).IsUnique();

            modelBuilder.Entity<Accommodation>().Property(a => a.DateAndTimeOfCreation)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            modelBuilder.Entity<Accommodation>().HasRequired(a => a.TypeOfAccommodation)
                .WithMany(toa => toa.Accommodations).WillCascadeOnDelete(true);

            //modelBuilder.Entity<Accommodation>().HasRequired(a => a.Chain).WithMany(ch => ch.Accommodations)
            //    .WillCascadeOnDelete(false);

            // modelBuilder.Entity<Accommodation>().HasOptional(a => a.Airport).WithMany(a => a.Accommodations).


            modelBuilder.Entity<Accommodation>().HasRequired(a => a.Creator).WithMany(user => user.Accommodations)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Accommodation>().HasRequired(a => a.Country).WithMany(c => c.Accommodations)
                .WillCascadeOnDelete(false);

        }

    }

}
