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
        public virtual IDbSet<PhotoOfAccommodationToTypeOfRoom> PhotosOfAccommodationsToTypesOfRooms { get; set; }
        public virtual IDbSet<TypeOfAttribute> TypesOfAttributes { get; set; }
        public virtual IDbSet<SubTypeOfAttribute> SubTypesOfAttributes { get; set; }
        public virtual IDbSet<Attribute> Attributes { get; set; }


        //public virtual IDbSet<Travel.EAN.DTO.Property.Attribute> Attributes { get; set; }


        public TravelContext() : base("name=Travel")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            OnUsersCreating(modelBuilder);
            OnLogsOfImportsCreating(modelBuilder);

            OnGeoCreating(modelBuilder, "geo");

            OnLanguagesCreating(modelBuilder);

            OnPathsToPhotosCreating(modelBuilder);
            OnFilesExtensionsCreating(modelBuilder);
            OnCaptionsCreating(modelBuilder);
            OnLocalizedCaptions(modelBuilder);

            OnAccoCreating(modelBuilder, "acco");

        }


        private void OnGeoCreating(DbModelBuilder modelBuilder, string dbSchema)
        {
            OnTypesOfRegionsCreating(modelBuilder, dbSchema, nameof(TypesOfRegions));
            OnRegionsCreating(modelBuilder, dbSchema, nameof(Regions));
            OnSubClassesCreating(modelBuilder, dbSchema, nameof(SubClasses));
            OnRegionsToTypes(modelBuilder, dbSchema, nameof(RegionsToTypes));
            OnLocalizedRegions(modelBuilder, dbSchema, nameof(LocalizedRegions));
            OnRegionsToRegionsCreating(modelBuilder, dbSchema, nameof(RegionsToRegions));
            OnCountriesCreating(modelBuilder, dbSchema, nameof(Countries));
            OnAirportsCreating(modelBuilder, dbSchema, nameof(Airports));
        }


        private void OnAccoCreating(DbModelBuilder modelBuilder, string dbSchema)
        {
            OnTypesOfAccommodationsCreating(modelBuilder, dbSchema, nameof(TypesOfAccommodations));
            OnLocalizedTypesOfAccommodationsCreating(modelBuilder, dbSchema, nameof(LocalizedTypesOfAccommodations));
            OnChainsCreating(modelBuilder, dbSchema, nameof(Chains));
            OnAccommodationsCreating(modelBuilder, dbSchema, nameof(Accommodations));
            OnLocalizedAccommodationsCreating(modelBuilder, dbSchema, nameof(LocalizedAccommodations));
            OnTypesOfDescriptionsCreating(modelBuilder, dbSchema, nameof(TypesOfDescriptions));
            OnDescriptionsCreating(modelBuilder, dbSchema, nameof(Descriptions));
            OnPhotosOfAccommodationsCreating(modelBuilder, dbSchema, nameof(PhotosOfAccommodations));
            OnTypesOfRoomsCreating(modelBuilder, dbSchema, nameof(TypesOfRooms));
            OnLocalizedTypesOfRoomsCreating(modelBuilder, dbSchema, nameof(LocalizedTypesOfRooms));
            OnPhotosOfAccommodationsToTypesOfRoomsCreating(modelBuilder, dbSchema,
                nameof(PhotosOfAccommodationsToTypesOfRooms));

            OnTypesOfAttributesCreating(modelBuilder, dbSchema, nameof(TypesOfAttributes));
            OnSubTypesOfAttributesCreating(modelBuilder, dbSchema, nameof(SubTypesOfAttributes));
            OnAttributesCreating(modelBuilder, dbSchema, nameof(Attributes));

        }

        private static void OnAttributesCreating(DbModelBuilder modelBuilder, string dbSchema, string tableName)
        {
            modelBuilder.Entity<Attribute>().ToTable(tableName, dbSchema);

            modelBuilder.Entity<Attribute>().HasRequired(a => a.TypeOfAttribute).WithMany(toa => toa.Attributes)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Attribute>().HasRequired(a => a.SubTypeOfAttribute).WithMany(toa => toa.Attributes)
                .WillCascadeOnDelete(false);
        }


        private static void OnSubTypesOfAttributesCreating(DbModelBuilder modelBuilder, string dbSchema, string tableName)
        {
            modelBuilder.Entity<SubTypeOfAttribute>().ToTable(tableName, dbSchema).HasIndex(p => p.Name).IsUnique();
        }


        private static void OnTypesOfAttributesCreating(DbModelBuilder modelBuilder, string dbSchema, string tableName)
        {
            modelBuilder.Entity<TypeOfAttribute>().ToTable(tableName, dbSchema).HasIndex(p => p.Name).IsUnique();
        }

        private static void OnPhotosOfAccommodationsToTypesOfRoomsCreating(DbModelBuilder modelBuilder, string dbSchema, string tableName)
        {
            modelBuilder.Entity<PhotoOfAccommodationToTypeOfRoom>().ToTable(tableName, dbSchema)
                .HasRequired(p => p.Creator).WithMany(u => u.PhotosOfAccommodationsToTypesOfRooms)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PhotoOfAccommodationToTypeOfRoom>().HasRequired(p => p.TypeOfRoom)
                .WithMany(tor => tor.PhotosOfAccommodationsToTypesOfRooms).HasForeignKey(p => p.ToId).WillCascadeOnDelete(false);

        }


        private static void OnLocalizedTypesOfRoomsCreating(DbModelBuilder modelBuilder, string dbSchema, string tableName)
        {
            modelBuilder.Entity<LocalizedTypeOfRoom>()
                .ToTable(tableName, dbSchema).HasRequired(ltor => ltor.Creator)
                .WithMany(u => u.LocalizedTypesOfRooms).WillCascadeOnDelete(false);

            modelBuilder.Entity<LocalizedTypeOfRoom>()
                .HasRequired(ltor => ltor.Language).WithMany(l => l.LocalizedTypesOfRooms).WillCascadeOnDelete(false);
        }


        private static void OnLocalizedCaptions(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LocalizedCaption>()
                .HasRequired(lc => lc.Language).WithMany(l => l.LocalizedCaptions).WillCascadeOnDelete(false);

            modelBuilder.Entity<LocalizedCaption>().HasRequired(lc => lc.Creator).WithMany(u => u.LocalizedCaptions)
                .WillCascadeOnDelete(false);

        }

        private static void OnTypesOfRoomsCreating(DbModelBuilder modelBuilder, string dbSchema, string tableName)
        {
            modelBuilder.Entity<TypeOfRoom>().ToTable(tableName, dbSchema)
                .HasRequired(tor => tor.Creator)
                .WithMany(u => u.TypesOfRooms)
                .WillCascadeOnDelete(false)
                ;

            modelBuilder.Entity<TypeOfRoom>().HasRequired(tor => tor.Accommodation)
                .WithMany(a => a.TypesOfRooms).WillCascadeOnDelete()
                ;
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

        private static void OnPhotosOfAccommodationsCreating(DbModelBuilder modelBuilder, string dbSchema, string tableName)
        {
            modelBuilder.Entity<PhotoOfAccommodation>().ToTable(tableName, dbSchema)
               .HasIndex(p => new { p.PathToPhotoId, p.FileName, p.FileExtensionId }).IsUnique();

            modelBuilder.Entity<PhotoOfAccommodation>().HasRequired(p => p.Accommodation).WithMany(p => p.PhotosOfAccommodations)
                .HasForeignKey(p => p.AccommodationId).WillCascadeOnDelete(true);

            modelBuilder.Entity<PhotoOfAccommodation>().HasRequired(poa => poa.PathToPhoto)
                .WithMany(ptp => ptp.PhotosOfAccommodations).WillCascadeOnDelete(false);

            modelBuilder.Entity<PhotoOfAccommodation>().HasRequired(poa => poa.FileExtension)
                .WithMany(fe => fe.PhotosOfAccommodations).WillCascadeOnDelete(false);

            modelBuilder.Entity<PhotoOfAccommodation>().HasRequired(poa => poa.Creator)
                .WithMany(u => u.PhotosOfAccommodations).WillCascadeOnDelete(false);
        }

        private static void OnDescriptionsCreating(DbModelBuilder modelBuilder, string dbSchema, string tableName)
        {
            modelBuilder.Entity<Description>().ToTable(tableName, dbSchema).Property(p => p.DateAndTimeOfCreation)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);


            modelBuilder.Entity<Description>().HasRequired(d => d.Accommodation)
                .WithMany(a => a.Descriptions).WillCascadeOnDelete(true);

            modelBuilder.Entity<Description>().HasRequired(d => d.TypeOfDescription).WithMany(tod => tod.Descriptions).WillCascadeOnDelete(false);

            modelBuilder.Entity<Description>().HasRequired(d => d.Language).WithMany(l => l.Descriptions).WillCascadeOnDelete(false);

            modelBuilder.Entity<Description>().HasRequired(d => d.Creator).WithMany(u => u.Descriptions).WillCascadeOnDelete(false);
        }


        private static void OnLocalizedAccommodationsCreating(DbModelBuilder modelBuilder, string dbSchema, string tableName)
        {
            modelBuilder.Entity<LocalizedAccommodation>()
                .ToTable(tableName, dbSchema).HasRequired(la => la.Creator)
                .WithMany(user => user.LocalizedAccommodations).WillCascadeOnDelete(false);

            modelBuilder.Entity<LocalizedAccommodation>().HasRequired(la => la.Accommodation)
                .WithMany(a => a.LocalizedAccommodations).WillCascadeOnDelete(true);

            modelBuilder.Entity<LocalizedAccommodation>().HasRequired(la => la.Language)
                .WithMany(l => l.LocalizedAccommodations).WillCascadeOnDelete(false);
        }


        private static void OnChainsCreating(DbModelBuilder modelBuilder, string dbSchema, string tableName)
        {
            modelBuilder.Entity<Chain>().ToTable(tableName, dbSchema).HasIndex(p => p.EanId).IsUnique();

            modelBuilder.Entity<Chain>().HasRequired(ch => ch.Creator).WithMany(user => user.Chains)
                .WillCascadeOnDelete(true);

        }

        private static void OnLocalizedTypesOfAccommodationsCreating(DbModelBuilder modelBuilder, string dbSchema, string tableName)
        {
            modelBuilder.Entity<LocalizedTypeOfAccommodation>().ToTable(tableName, dbSchema)
                .HasRequired(ltoa => ltoa.Creator).WithMany(user => user.LocalizedTypesOfAccommodations).WillCascadeOnDelete(false);

            modelBuilder.Entity<LocalizedTypeOfAccommodation>().HasRequired(ltoa => ltoa.TypeOfAccommodation)
                .WithMany(toa => toa.LocalizedTypesOfAccommodations).WillCascadeOnDelete(true);

            modelBuilder.Entity<LocalizedTypeOfAccommodation>().HasRequired(ltoa => ltoa.Language)
                .WithMany(l => l.LocalizedTypesOfAccommodations).WillCascadeOnDelete(false);
        }

        private static void OnTypesOfAccommodationsCreating(DbModelBuilder modelBuilder, string dbSchema, string tableName)
        {
            modelBuilder.Entity<TypeOfAccommodation>()
                .ToTable(tableName, dbSchema).HasIndex(toa => toa.EanId).IsUnique();
        }

        private static void OnRegionsToTypes(DbModelBuilder modelBuilder, string dbSchema, string tableName)
        {
            modelBuilder.Entity<RegionToType>().ToTable(tableName, dbSchema).HasRequired(rtp => rtp.Creator)
                .WithMany(user => user.RegionsToTypes).WillCascadeOnDelete(false);

            modelBuilder.Entity<RegionToType>().HasRequired(rtp => rtp.Region).WithMany(region => region.RegionsToTypes)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<RegionToType>().HasRequired(rtt => rtt.TypeOfRegion).WithMany(tor => tor.RegionsToTypes)
                .HasForeignKey(p => p.ToId).WillCascadeOnDelete(false);

        }

        private static void OnAirportsCreating(DbModelBuilder modelBuilder, string dbSchema, string tableName)
        {
            modelBuilder.Entity<Airport>()
                .ToTable(tableName, dbSchema).HasRequired(c => c.Creator).WithMany(user => user.Airports).WillCascadeOnDelete(false);

            modelBuilder.Entity<Airport>().HasIndex(c => c.Id).IsUnique();
            modelBuilder.Entity<Airport>().HasIndex(c => c.Code).IsUnique();
            modelBuilder.Entity<Airport>().HasRequired(c => c.Region).WithOptional(r => r.AdditionalAirportProperties).WillCascadeOnDelete(true);
        }


        private static void OnCountriesCreating(DbModelBuilder modelBuilder, string dbSchema, string tableName)
        {
            modelBuilder.Entity<Country>().ToTable(tableName, dbSchema).HasRequired(c => c.Creator)
                .WithMany(user => user.Countries).WillCascadeOnDelete(false);

            modelBuilder.Entity<Country>().HasIndex(c => c.Id).IsUnique();
            modelBuilder.Entity<Country>().HasIndex(c => c.Code).IsUnique();

            modelBuilder.Entity<Country>().HasRequired(c => c.Region).WithOptional(r => r.AdditionalCountryProperties).WillCascadeOnDelete(true);
        }

        private static void OnRegionsToRegionsCreating(DbModelBuilder modelBuilder, string dbSchema, string tableName)
        {
            modelBuilder.Entity<RegionToRegion>()
                .ToTable(tableName, dbSchema).HasRequired(rtr => rtr.Creator)
                .WithMany(u => u.RegionsToRegions).WillCascadeOnDelete(false);

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

        }


        private static void OnLocalizedRegions(DbModelBuilder modelBuilder, string dbSchema, string tableName)
        {
            modelBuilder.Entity<LocalizedRegion>().ToTable(tableName, dbSchema).HasRequired(lr => lr.Creator)
                .WithMany(user => user.LocalizedRegions).WillCascadeOnDelete(false);

            modelBuilder.Entity<LocalizedRegion>()
                .HasRequired(lr => lr.Language)
                .WithMany(l => l.LocalizedRegions)
                .WillCascadeOnDelete(false);

        }


        private static void OnRegionsCreating(DbModelBuilder modelBuilder, string dbSchema, string tableName)
        {
            modelBuilder.Entity<Region>().ToTable(tableName, dbSchema).HasIndex(p => p.EanId).IsUnique();

            modelBuilder.Entity<Region>().HasRequired(r => r.Creator).WithMany(u => u.Regions).WillCascadeOnDelete(false);

        }

        private static void OnTypesOfRegionsCreating(DbModelBuilder modelBuilder, string dbSchema, string tableName)
        {
            modelBuilder.Entity<TypeOfRegion>()
                .ToTable(tableName, dbSchema)
                .HasIndex(typeOfRegion => typeOfRegion.Name).IsUnique();

        }

        private static void OnSubClassesCreating(DbModelBuilder modelBuilder, string dbSchema, string tableName)
        {
            modelBuilder.Entity<SubClass>().ToTable(tableName, dbSchema).HasIndex(p => p.Name).IsUnique();

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


        private static void OnTypesOfDescriptionsCreating(DbModelBuilder modelBuilder, string dbSchema, string tableName)
        {
            modelBuilder.Entity<TypeOfDescription>()
                .ToTable(tableName, dbSchema)
                .HasIndex(p => p.Name).IsUnique();

            modelBuilder.Entity<TypeOfDescription>().HasRequired(tod => tod.Creator)
                .WithMany(user => user.TypesOfDescriptions).WillCascadeOnDelete(true);
        }


        private void OnAccommodationsCreating(DbModelBuilder modelBuilder, string dbSchema, string tableName)
        {
            modelBuilder.Entity<Accommodation>().ToTable(tableName, dbSchema).HasIndex(e => e.EanId).IsUnique();

            modelBuilder.Entity<Accommodation>().HasRequired(a => a.TypeOfAccommodation)
                .WithMany(toa => toa.Accommodations).WillCascadeOnDelete(true);

            modelBuilder.Entity<Accommodation>().HasRequired(a => a.Creator).WithMany(user => user.Accommodations)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Accommodation>().HasRequired(a => a.Country).WithMany(c => c.Accommodations)
                .WillCascadeOnDelete(false);

        }

    }

}
