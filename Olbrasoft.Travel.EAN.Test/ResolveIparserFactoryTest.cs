using System;
using System.Text;
using System.Collections.Generic;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Olbrasoft.Travel.EAN.Test
{
    /// <summary>
    /// Summary description for ResolveIparserFactoryTest
    /// </summary>
    [TestClass]
    public class ResolveIparserFactoryTest
    {
        public ResolveIparserFactoryTest()
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
        // public static void MyClassInitialize(TestContext testContext) { }
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
        public void TestMethod1()
        {
            //Arrange
            var container = new WindsorContainer();

            container.Register(Component.For<IParserFactory>().ImplementedBy<ParserFactory>());

            //Act
            var factory = container.Resolve<IParserFactory>();


            //Assert
            Assert.IsNotNull(factory);

            var parser = factory.Create<Person>("Id|Name|SurName");

            Assert.IsTrue(parser.TryParse("5|Lenka|Tůmová", out var person));


            Assert.IsTrue(person.Name=="Lenka");

        }
    }
}
