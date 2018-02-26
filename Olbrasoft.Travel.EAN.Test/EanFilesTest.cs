using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Olbrasoft.Travel.EAN.Test
{
    /// <summary>
    /// Summary description for EanFilesTest
    /// </summary>
    [TestClass]
    public class EanFilesTest
    {
        
        [TestMethod]
        public void TestMethod1()
        {
            //Arrange
            var geographyTypes = System.Reflection.Assembly.Load("Olbrasoft.Travel.EAN").GetTypes()
                .Where(t => t.Namespace == "Olbrasoft.Travel.EAN.DTO.Geography");
            var propertyTypes = System.Reflection.Assembly.Load("Olbrasoft.Travel.EAN").GetTypes()
                .Where(t => t.Namespace == "Olbrasoft.Travel.EAN.DTO.Property");


            //Act
            var eanFiles= EanFiles.GetEanFiles();
            var geographyTypesCount = geographyTypes.Count();
            var propertyTypesCount = propertyTypes.Count();
            

            //Asert
            Assert.IsTrue(geographyTypesCount== eanFiles.Count(p => p.TypeOfEanFile==TypeOfEanFile.Geography));
            Assert.IsTrue(propertyTypesCount == eanFiles.Count(p => p.TypeOfEanFile == TypeOfEanFile.Property));

         
        }
    }
}
