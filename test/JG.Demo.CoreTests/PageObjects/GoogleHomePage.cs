using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;

namespace JG.Demo.CoreTests.PageObjects
{
    public class GoogleHomePage : DemoPage
    {
        public GoogleHomePage(IWebDriver driver, Uri baseUrl, string path) : base(driver, baseUrl, path)
        {
        }

        public IWebElement Doodle
        {
            get { return this.Driver.FindElement(By.XPath(@"//*[@id=""hplogo""]")); }
        }
    }
}
