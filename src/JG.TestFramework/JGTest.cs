﻿using System;
using System.IO;
using System.Reflection;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace JG.TestFramework
{
    [TestClass]
    public class JGTest
    {
        private static IWebDriverFactory factory;
        private static DriverService service;
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
        public static void AssemblyInitialize(TestContext context)
        {
            var webDriverType = (string)context.Properties["WebDriver"];
            var executableLocation = (string)context.Properties["ExecutableLocation"];
            var baseUrl = (string)context.Properties["BaseUrl"];

            var workingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            switch (webDriverType)
            {
                case "firefox":
                    var firefoxOptions = new FirefoxOptions();
                    firefoxOptions.BrowserExecutableLocation = executableLocation;
                    firefoxOptions.AddArguments(new string[]
                    {
                        "--headless"
                    });
                    JGTest.factory = new FirefoxDriverFactory(workingDirectory, firefoxOptions, TimeSpan.FromSeconds(60));
                    break;
                case "chrome":
                default:
                    var chromeOptions = new ChromeOptions();
                    chromeOptions.AddArguments(new string[]
                    {
                        "--no-sandbox",
                        "--headless",
                        "--disable-gpu"
                    });
                    var chromeService = ChromeDriverService.CreateDefaultService(workingDirectory);
                    chromeService.Start();
                    JGTest.service = chromeService;
                    JGTest.factory = new ChromeDriverFactory(chromeService, chromeOptions, TimeSpan.FromSeconds(60));
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
            if(service !=  null) service.Dispose();
        }
    }
}