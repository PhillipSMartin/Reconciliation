using ReconciliationConsole;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ReconciliationLib;

namespace ReconciliationTest
{
    
    
    /// <summary>
    ///This is a test class for ProgramTest and is intended
    ///to contain all ProgramTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ProgramTest
    {


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
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for IsFullyPaidLendingAccount
        ///</summary>
        [TestMethod()]
        public void IsFullyPaidLendingAccountTest()
        {
            string fileName = "GARG_DIVRECON_06-31016.xml";
            ClearingHouse clearingHouse = ClearingHouse.WellsFargo;
            bool expected = true;
            bool actual;
            actual = Program.IsFullyPaidLendingAccount(fileName, clearingHouse);
            Assert.AreEqual(expected, actual);

            fileName = "47881029_DIVRECON_06-31016.xml";
            clearingHouse = ClearingHouse.WellsFargo;
            expected = false;
            actual = Program.IsFullyPaidLendingAccount(fileName, clearingHouse);
            Assert.AreEqual(expected, actual);

            fileName = "GARG_DIVRECON_06-31016.xml";
            clearingHouse = ClearingHouse.BONY;
            expected = false;
            actual = Program.IsFullyPaidLendingAccount(fileName, clearingHouse);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void IsFuturesAccountTest()
        {
            string fileName = "2MA00012_POS_05082018.xml";
            ClearingHouse clearingHouse = ClearingHouse.WellsFargo;
            bool expected = false;
            bool actual;
            actual = Program.IsFuturesAccount(fileName, clearingHouse);
            Assert.AreEqual(expected, actual);

            fileName = "47881467_POS_05082018.xml";
            clearingHouse = ClearingHouse.WellsFargo;
            expected = true;
            actual = Program.IsFuturesAccount(fileName, clearingHouse);
            Assert.AreEqual(expected, actual);

        }
    }
}
