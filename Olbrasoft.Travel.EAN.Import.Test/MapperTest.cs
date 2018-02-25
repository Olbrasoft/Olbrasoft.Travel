using System;
using System.Data.Entity.Spatial;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Olbrasoft.Travel.EAN.Import.Test
{
    [TestClass]
    public class MapperTest
    {
        [TestMethod]
        public void ParsePoligonTest()
        {
            var text = "31.144562;33.741344:31.171932;33.873543:31.103771;33.884808:31.073994;33.786865254";
           
            Assert.IsTrue(text.Length>0);
        }

    }
}
