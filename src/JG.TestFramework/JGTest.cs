using System;
using System.IO;
using System.Reflection;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace JG.TestFramework
{
    [TestClass]
    public class JGTest
    {
        private static IWebDriverFactory factory;
        private static Uri baseUrl;
        private static ThreadLocal<IWebDriver> driver = new ThreadLocal<IWebDriver>(() => factory.Create());

        public IWebDriver Driver
        {
            get
            {
                return JGTest.driver.Value;
            }
        }

        public Uri BaseUrl
        {
            get
            {
                return JGTest.baseUrl;
            }
        }

        public TestContext TestContext { get; set; }

        //[AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            var webDriverType = (string)context.Properties["WebDriver"];
            var baseUrl = (string)context.Properties["BaseUrl"];

            switch (webDriverType)
            {
                case "chrome":
                default:
                    var workingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    var options = new ChromeOptions();
                    options.AddArguments(new string[]
                    {
                        "--no-sandbox",
                        "--headless",
                        "--disable-gpu"
                    });
                    JGTest.factory = new ChromeDriverFactory(workingDirectory, options, TimeSpan.FromSeconds(60));
                    break;
            }

            JGTest.baseUrl = new Uri(baseUrl);
        }

        [TestInitialize]
        public virtual void TestInitialize()
        {
        }

        [TestCleanup]
        public virtual void TestCleanup()
        {
            driver.Value.Dispose();
        }

        //[AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            driver.Dispose();
        }
    }
}
