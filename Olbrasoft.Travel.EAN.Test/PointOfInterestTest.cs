using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Olbrasoft.Travel.EAN.DTO;
using Olbrasoft.Travel.EAN.DTO.Geography;

namespace Olbrasoft.Travel.EAN.Test
{
    /// <summary>
    /// Summary description for PointOfInterestTest
    /// </summary>
    [TestClass]
    public class PointOfInterestTest
    {
       

      
        [TestMethod]
        public void PointOfInterestValidateTest()
        {
            //Arrange
            //704|Barter Island|Barter Island, Alaska, United States of America|70.129562|-143.662949|tree
            PointOfInterest pointOfInterest =new PointOfInterest
            {
                RegionID = 704,
                RegionName = "Barter Island",
                RegionNameLong = "Barter Island, Alaska, United States of America",
                Latitude = 70.129562,
                Longitude = -143.662949,
                SubClassification = "tree"
            };


            //act
            var validationResults = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(
                pointOfInterest, new ValidationContext(pointOfInterest), validationResults, true);

            // Assert
            Assert.IsTrue(actual, "Expected validation to succeed.");
            Assert.AreEqual(0, validationResults.Count, "Unexpected number of validation errors.");


        }


        [TestMethod]
        public void PointOfInterestValidateFalseTest()
        {
            //Arrange
            //704|Barter Island|Barter Island, Alaska, United States of America|70.129562|-143.662949|tree
            PointOfInterest pointOfInterest = new PointOfInterest
            {
                RegionID = 704,
                //RegionName = "Barter Island",
                RegionNameLong = "Barter Island, Alaska, United States of America",
                Latitude = 95,  //range of -90 90
                Longitude = -143.662949,
                SubClassification = "tree"
            };


            //act
            var validationResults = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(
                pointOfInterest, new ValidationContext(pointOfInterest), validationResults, true);

            // Assert
            Assert.IsFalse(actual, "Expected validation to succeed.");
            Assert.AreEqual(2, validationResults.Count, "Unexpected number of validation errors.");
            
        }
    }
}
