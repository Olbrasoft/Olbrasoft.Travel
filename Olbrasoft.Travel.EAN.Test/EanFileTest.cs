using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Olbrasoft.Travel.EAN.DTO.Geography;

namespace Olbrasoft.Travel.EAN.Test
{
    /// <summary>
    /// Summary description for EanFileTest
    /// </summary>
    [TestClass]
    public class EanFileTest
    {
     [TestMethod]
        public void GetIsMultiLanguageTest()
        {
            //Arrrange
            var url = new Uri("https://www.ian.com/affiliatecenter/include/V2/new/RegionList_xx_XX.zip");

            //Act
            var multiLanguage = EanFile.GetIsMultilanguage(url);


            //Assert
            Assert.IsTrue(multiLanguage);

        }


        [TestMethod]
        public void IsMultilanguageTest()
        {
            //Arrange
            var eanMultilanguageFile = new EanFile(
                new Uri("https://www.ian.com/affiliatecenter/include/V2/new/RegionList_xx_XX.zip"),
                TypeOfEanFile.Geography,
                typeof(RegionMultiLanguage)
            );

            var eanSingleLanguageEanFile = new EanFile(
                new Uri("https://www.ian.com/affiliatecenter/include/V2/new/ParentRegionList.zip"),
                TypeOfEanFile.Geography,
                typeof(ParentRegion));
            
            //Act
            var multilanguage = eanMultilanguageFile.IsMultilanguage;
            var singleLanguage = eanSingleLanguageEanFile.IsMultilanguage;

            //Assert
            Assert.IsTrue(multilanguage);
            Assert.IsFalse(singleLanguage);


        }

    }
}
