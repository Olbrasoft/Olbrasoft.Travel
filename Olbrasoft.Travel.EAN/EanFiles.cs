using System;
using System.Collections.Generic;
using Olbrasoft.Travel.EAN.DTO.Geography;
using Olbrasoft.Travel.EAN.DTO.Property;

namespace Olbrasoft.Travel.EAN
{
    public class EanFiles
    {
        private static IEnumerable<EanFile> _eanFiles;

        public static IEnumerable<EanFile>  GetEanFiles()
        {
            return _eanFiles ?? (_eanFiles = BuildEanFiles());
        }
        
        private static IEnumerable<EanFile> BuildEanFiles()
        {
            var files = new List<EanFile>()
            {
                //Geography https://support.ean.com/hc/en-us/articles/115005784405

                new EanFile(
                    new Uri("https://www.ian.com/affiliatecenter/include/V2/new/ParentRegionList.zip"),
                    TypeOfEanFile.Geography,
                    typeof(ParentRegion)
                ),

                new EanFile(
                    new Uri("https://www.ian.com/affiliatecenter/include/V2/new/RegionList_xx_XX.zip"),
                    TypeOfEanFile.Geography,
                    typeof(RegionMultiLanguage)
                ),
                new EanFile(
                    new Uri("https://www.ian.com/affiliatecenter/include/V2/new/CountryList.zip"),
                    TypeOfEanFile.Geography,
                    typeof(Country)
                ),
                new EanFile(
                    new Uri("https://www.ian.com/affiliatecenter/include/V2/new/CountryList_xx_XX.zip"),
                    TypeOfEanFile.Geography,
                    typeof(CountryMultiLanguage)
                ),
                new EanFile(
                    new Uri("https://www.ian.com/affiliatecenter/include/V2/new/CityCoordinatesList.zip"),
                    TypeOfEanFile.Geography,
                    typeof(CityCoordinates)
                ),
                new EanFile(
                    new Uri("https://www.ian.com/affiliatecenter/include/V2/new/NeighborhoodCoordinatesList.zip"),
                    TypeOfEanFile.Geography,
                    typeof(NeighborhoodCoordinates)
                ),
                new EanFile(
                    new Uri("https://www.ian.com/affiliatecenter/include/V2/new/PointsOfInterestCoordinatesList.zip"),
                    TypeOfEanFile.Geography,
                    typeof(PointOfInterestCoordinates)
                ),
                new EanFile(
                    new Uri("https://www.ian.com/affiliatecenter/include/V2/new/AirportCoordinatesList.zip"),
                    TypeOfEanFile.Geography,
                    typeof(AirportCoordinates)
                ),
                new EanFile(
                    new Uri("https://www.ian.com/affiliatecenter/include/V2/new/TrainMetroStationCoordinatesList.zip"),
                    TypeOfEanFile.Geography,
                    typeof(TrainMetroStationCoordinates)
                ),
                new EanFile(
                    new Uri("https://www.ian.com/affiliatecenter/include/V2/new/RegionCenterCoordinatesList.zip"),
                    TypeOfEanFile.Geography,
                    typeof(RegionCenter)
                ),
                new EanFile(
                    new Uri("https://www.ian.com/affiliatecenter/include/V2/new/RegionEANHotelIDMapping.zip"),
                    TypeOfEanFile.Geography,
                    typeof(RegionEANHotelIDMapping)
                ),

                //Property https://support.ean.com/hc/en-us/articles/115005784325

                new EanFile(
                    new Uri("https://www.ian.com/affiliatecenter/include/V2/ActivePropertyList.zip"),
                    TypeOfEanFile.Property,
                    typeof(ActiveProperty)
                ),
                new EanFile(
                    new Uri("https://www.ian.com/affiliatecenter/include/V2/ActivePropertyList_xx_XX.zip"),
                    TypeOfEanFile.Property,
                    typeof(ActivePropertyMultiLanguage)
                ),
                new EanFile(
                    new Uri("https://www.ian.com/affiliatecenter/include/V2/PropertyTypeList.zip"),
                    TypeOfEanFile.Property,
                    typeof(PropertyType)
                ),
                new EanFile(
                    new Uri("https://www.ian.com/affiliatecenter/include/V2/PropertyTypeList_xx_XX.zip"),
                    TypeOfEanFile.Property,
                    typeof(PropertyTypeMultiLanguage)
                ),
                new EanFile(
                    new Uri("https://www.ian.com/affiliatecenter/include/V2/ChainList.zip"),
                    TypeOfEanFile.Property,
                    typeof(Chain)
                ),
                new EanFile(
                    new Uri("https://www.ian.com/affiliatecenter/include/V2/PropertyDescriptionList.zip"),
                    TypeOfEanFile.Property,
                    typeof(Description)
                ),
                new EanFile(
                    new Uri("https://www.ian.com/affiliatecenter/include/V2/PropertyDescriptionList_xx_XX.zip"),
                    TypeOfEanFile.Property,
                    typeof(DescriptionMultiLanguage)
                ),
                new EanFile(
                    new Uri("https://www.ian.com/affiliatecenter/include/V2/PolicyDescriptionList.zip"),
                    TypeOfEanFile.Property,
                    typeof(PolicyDescription)
                ),
                new EanFile(
                    new Uri("https://www.ian.com/affiliatecenter/include/V2/PolicyDescriptionList_xx_XX.zip"),
                    TypeOfEanFile.Property,
                    typeof(PolicyDescriptionMultiLanguage)
                ),
                new EanFile(
                    new Uri("https://www.ian.com/affiliatecenter/include/V2/PropertyFeesList.zip"),
                    TypeOfEanFile.Property,
                    typeof(PropertyFees)
                ),
                new EanFile(
                    new Uri("https://www.ian.com/affiliatecenter/include/V2/PropertyFeesList_xx_XX.zip"),
                    TypeOfEanFile.Property,
                    typeof(PropertyFeesMultiLanguage)
                ),
                new EanFile(
                    new Uri("https://www.ian.com/affiliatecenter/include/V2/PropertyMandatoryFeesList.zip"),
                    TypeOfEanFile.Property,
                    typeof(PropertyMandatoryFees)
                ),
                new EanFile(
                    new Uri("https://www.ian.com/affiliatecenter/include/V2/PropertyMandatoryFeesList_xx_XX.zip"),
                    TypeOfEanFile.Property,
                    typeof(PropertyMandatoryFeesMultiLanguage)
                ),
                new EanFile(
                    new Uri("https://www.ian.com/affiliatecenter/include/V2/RecreationDescriptionList.zip"),
                    TypeOfEanFile.Property,
                    typeof(RecreationDescription)
                ),
                new EanFile(
                    new Uri("https://www.ian.com/affiliatecenter/include/V2/RecreationDescriptionList_xx_XX.zip"),
                    TypeOfEanFile.Property,
                    typeof(RecreationDescriptionMultiLanguage)
                ),
                new EanFile(
                    new Uri("https://www.ian.com/affiliatecenter/include/V2/PropertyAmenitiesList.zip"),
                    TypeOfEanFile.Property,
                    typeof(Amenities)
                ),
                new EanFile(
                    new Uri("https://www.ian.com/affiliatecenter/include/V2/PropertyAmenitiesList_xx_XX.zip"),
                    TypeOfEanFile.Property,
                    typeof(AmenitiesMultiLanguage)
                ),
                    new EanFile(
                    new Uri("https://www.ian.com/affiliatecenter/include/V2/PropertyBusinessAmenitiesList.zip"),
                    TypeOfEanFile.Property,
                    typeof(BusinessAmenities)
                ),
                new EanFile(
                    new Uri("https://www.ian.com/affiliatecenter/include/V2/PropertyBusinessAmenitiesList_xx_XX.zip"),
                    TypeOfEanFile.Property,
                    typeof(BusinessAmenitiesMultiLanguage)
                ),
                new EanFile(
                    new Uri("https://www.ian.com/affiliatecenter/include/V2/PropertyLocationList.zip"),
                    TypeOfEanFile.Property,
                    typeof(Location)
                ),
                new EanFile(
                    new Uri("https://www.ian.com/affiliatecenter/include/V2/PropertyLocationList_xx_XX.zip"),
                    TypeOfEanFile.Property,
                    typeof(LocationMultiLanguage)
                ),
                new EanFile(
                    new Uri("https://www.ian.com/affiliatecenter/include/V2/PropertyNationalRatingsList.zip"),
                    TypeOfEanFile.Property,
                    typeof(NationalRatings)
                ),
                new EanFile(
                    new Uri("https://www.ian.com/affiliatecenter/include/V2/PropertyNationalRatingsList_xx_XX.zip"),
                    TypeOfEanFile.Property,
                    typeof(NationalRatingsMultiLanguage)
                ),
                new EanFile(
                    new Uri("https://www.ian.com/affiliatecenter/include/V2/PropertyRenovationsList.zip"),
                    TypeOfEanFile.Property,
                    typeof(Renovations)
                ),
                new EanFile(
                    new Uri("https://www.ian.com/affiliatecenter/include/V2/PropertyRenovationsList_xx_XX.zip"),
                    TypeOfEanFile.Property,
                    typeof(RenovationsMultiLanguage)
                ),
                new EanFile(
                    new Uri("https://www.ian.com/affiliatecenter/include/V2/PropertyRoomsList.zip"),
                    TypeOfEanFile.Property,
                    typeof(Rooms)
                ),
                new EanFile(
                    new Uri("https://www.ian.com/affiliatecenter/include/V2/PropertyRoomsList_xx_XX.zip"),
                    TypeOfEanFile.Property,
                    typeof(RoomsMultiLanguage)
                ),
                new EanFile(
                    new Uri("https://www.ian.com/affiliatecenter/include/V2/AreaAttractionsList.zip"),
                    TypeOfEanFile.Property,
                    typeof(AreaAttractions)
                ),
                new EanFile(
                    new Uri("https://www.ian.com/affiliatecenter/include/V2/AreaAttractionsList_xx_XX.zip"),
                    TypeOfEanFile.Property,
                    typeof(AreaAttractionsMultiLanguage)
                ),
                new EanFile(
                    new Uri("https://www.ian.com/affiliatecenter/include/V2/DiningDescriptionList.zip"),
                    TypeOfEanFile.Property,
                    typeof(DiningDescription)
                ),
                new EanFile(
                    new Uri("https://www.ian.com/affiliatecenter/include/V2/DiningDescriptionList_xx_XX.zip"),
                    TypeOfEanFile.Property,
                    typeof(DiningDescriptionMultiLanguage)
                ),
                new EanFile(
                    new Uri("https://www.ian.com/affiliatecenter/include/V2/SpaDescriptionList.zip"),
                    TypeOfEanFile.Property,
                    typeof(SpaDescription)
                ),
                new EanFile(
                    new Uri("https://www.ian.com/affiliatecenter/include/V2/SpaDescriptionList_xx_XX.zip"),
                    TypeOfEanFile.Property,
                    typeof(SpaDescriptionMultiLanguage)
                ),
                new EanFile(
                    new Uri("https://www.ian.com/affiliatecenter/include/V2/WhatToExpectList.zip"),
                    TypeOfEanFile.Property,
                    typeof(WhatToExpectDescription)
                ),
                new EanFile(
                    new Uri("https://www.ian.com/affiliatecenter/include/V2/WhatToExpectList_xx_XX.zip"),
                    TypeOfEanFile.Property,
                    typeof(WhatToExpectMultiLanguage)
                ),

            };

            return files;
        }
    }
}