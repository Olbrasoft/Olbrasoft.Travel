using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Olbrasoft.Travel.EAN.Test
{
    /// <summary>
    /// Summary description for ParserTest
    /// </summary>
    [TestClass]
    public class ParserTest
    {
        public ParserTest()
        {
            var line = "Id|Name|SurName";
            Parser = new Parser<Person>(line);
        }

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public Parser<Person> Parser { get; set; }

       
        [TestMethod]
        public void HelloParser()
        {

            //Arrange
            Parser<Person> parser;

            //Act
            parser = new Parser<Person>("1|a|b");

           //Asssert                 
           Assert.IsNotNull(parser);
        }

        [TestMethod]
        public void Parser_TryParse_Test()
        {
            //Arrange
            var line = "100|Jiří|Tůma";

            //Act
            var result = Parser.TryParse(line, out var person);

            //Assert
            Assert.IsTrue(result);
        }


        [TestMethod]
        public void Parser_TryParse_False_Test()
        {
            //Name is Required
            //Arrange
            var line = "100||Tůma";

            //Act
            var result = Parser.TryParse(line, out var person);

            //Assert
            Assert.IsFalse(result); 
        }
    }
}
