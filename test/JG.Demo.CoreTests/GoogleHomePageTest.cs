using System;
using JG.Demo.CoreTests.PageObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace JG.Demo.CoreTests
{
    [TestClass]
    public class GoogleHomePageTest : DemoTest
    {
        [TestMethod]
        public void GoogleHomePage_Visited_DoodleDisplayed()
        {
             var homePage = new GoogleHomePage(this.Driver, this.BaseUrl, "/");
            homePage.Navigate();

            System.Threading.Thread.Sleep(10000);
            var displayed = homePage.Doodle.Displayed;

            Assert.IsTrue(displayed, "Doodle is not displayed");
        }

        [TestMethod]
        public void GoogleHomePage_Visited_TitleIsGoogle()
        {
            var homePage = new GoogleHomePage(this.Driver, this.BaseUrl, "/");
            homePage.Navigate();

            System.Threading.Thread.Sleep(10000);
            var text = homePage.Title;

            Assert.AreEqual("Google", text, false);
        }
    }
}
