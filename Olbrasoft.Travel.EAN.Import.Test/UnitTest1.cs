using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.EAN.Import.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //Arange
            var list = new List<LocalizedCountry>
            {
                new LocalizedCountry()
                {
                    Id = 1,
                    CreatorId = 1,
                    Name = "country1",
                    DateAndTimeOfCreation = DateTime.Now,
                    LanguageId = 1033
                },
                new LocalizedCountry()
                {
                    Id = 1,
                    CreatorId = 1,
                    Name = "country2",
                    DateAndTimeOfCreation = DateTime.Now,
                    LanguageId = 1033
                },
                new LocalizedCountry()
                {
                    Id = 1,
                    CreatorId = 1,
                    Name = "country3",
                    DateAndTimeOfCreation = DateTime.Now,
                    LanguageId = 1029
                },
                new LocalizedCountry()
                {
                    Id = 1,
                    CreatorId = 1,
                    Name = "country4",
                    DateAndTimeOfCreation = DateTime.Now,
                    LanguageId = 1033
                },
                new LocalizedCountry()
                {
                    Id = 1,
                    CreatorId = 1,
                    Name = "country5",
                    DateAndTimeOfCreation = DateTime.Now,
                    LanguageId = 21
                },
                new LocalizedCountry()
                {
                    Id = 1,
                    CreatorId = 1,
                    Name = "country6",
                    DateAndTimeOfCreation = DateTime.Now,
                    LanguageId = 1033
                }
            };

            //Act
            var result= list.GroupBy(test => test.LanguageId)
                .Select(grp => grp.First())
                .ToList();

            //Assert
            Assert.IsTrue(result.Count==3);

        }

        [TestMethod]
        public void HashSetTupleContainsTest()
        {
            //Arrange
            var hashSet= new HashSet<Tuple<int,int>>();
            hashSet.Add(new Tuple<int, int>(1, 1));
            hashSet.Add(new Tuple<int, int>(1, 2));
            var tup= new Tuple<int,int>(1,1);

            //Act
            var result = hashSet.Contains(tup);

            
            //Assert
            Assert.IsTrue(result);
        }

    }
}
