using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Olbrasoft.Travel.EAN.DTO;
using Olbrasoft.Travel.EAN.DTO.Geography;

namespace Olbrasoft.Travel.EAN.Test
{
    /// <summary>
    /// Summary description for ParentRegionTest
    /// </summary>
    [TestClass]
    public class ParentRegionTest
    {
        public ParentRegionTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(Parser testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void ValidateTrueTest()
        {
            //Arrange
            var parentRegion = new ParentRegion()
            {
                RegionID = 1,
                RegionName = "TestName",
                RegionType = "TestType",
                RegionNameLong = "RegionLongNameTest",
                RelativeSignificance = "abc"

            };

            //act
            var validationResults = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(
                parentRegion, new ValidationContext(parentRegion), validationResults,true);
            
            // Assert
            Assert.IsTrue(actual, "Expected validation to succeed.");
            Assert.AreEqual(0, validationResults.Count, "Unexpected number of validation errors.");
        }

        [TestMethod]
        public void ValidateFalseTest()
        {
            //Arrange
            var parentRegion = new ParentRegion()
            {
                RegionID = 1,
                RegionType = "TestType",
                RegionName = "TestName",
                RegionNameLong = "RegionLongNameTest",
                RelativeSignificance = "abcd" //error StringLength(3)

            };

            //Act
            var validationResults = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(
                parentRegion, new ValidationContext(parentRegion), validationResults,true);
            
            //Assert
            Assert.IsFalse(actual, "Expected validation to succeed.");
            Assert.AreEqual(1, validationResults.Count, "Unexpected number of validation errors.");

        }



    }
}
