using System;
using JG.TestFramework;
using OpenQA.Selenium;

namespace JG.Demo.CoreTests
{
    public class DemoPage : JGPage
    {
        public DemoPage(IWebDriver driver, Uri baseUrl, string path) : base(driver, baseUrl, path)
        {
        }

        public string Title
        {
            get { return this.Driver.Title; }
        }
    }
}
