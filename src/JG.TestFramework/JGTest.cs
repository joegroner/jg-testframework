using System;
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
        private class WebDriverWrapper : IDisposable
        {
            private IWebDriver wrapped;
            private IWebDriverFactory factory;

            public WebDriverWrapper(IWebDriverFactory factory)
            {
                this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
            }

            public IWebDriver Wrapped
            {
                get
                {
                    if (this.wrapped == null)
                    {
                        this.wrapped = this.factory.Create();
                    }

                    return this.wrapped;
                }
            }

            public void Dispose()
            {
                this.wrapped.Dispose();
                this.wrapped = null;
            }
        }

        private static IWebDriverFactory factory;
        //private static DriverService service;
        private static Uri baseUrl;
        private static ThreadLocal<WebDriverWrapper> driver = new ThreadLocal<WebDriverWrapper>(() => new WebDriverWrapper(factory));

        public IWebDriver Driver
        {
            get
            {
                return JGTest.driver.Value.Wrapped;
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
            var commandTimeout = int.Parse((string)context.Properties["CommandTimeout"]);
            var seleniumHost = (string)context.Properties["SeleniumHost"];

            var workingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            switch (webDriverType)
            {
                case "remotefirefox":
                    var remoteFirefoxOptions = new FirefoxOptions();
                    remoteFirefoxOptions.BrowserExecutableLocation = executableLocation;
                    remoteFirefoxOptions.AddArguments(new string[]
                    {
                        "--headless"
                    });
                    JGTest.factory = new RemoteDriverFactory(new Uri(seleniumHost), remoteFirefoxOptions.ToCapabilities(), TimeSpan.FromSeconds(commandTimeout));
                    break;
                case "remotechrome":
                    var remoteChromeOptions = new ChromeOptions();
                    remoteChromeOptions.AddArguments(new string[]
                    {
                        "--no-sandbox",
                        "--headless",
                        "--disable-gpu"
                    });
                    JGTest.factory = new RemoteDriverFactory(new Uri(seleniumHost), remoteChromeOptions.ToCapabilities(), TimeSpan.FromSeconds(commandTimeout));
                    break;
                case "firefox":
                    var firefoxOptions = new FirefoxOptions();
                    firefoxOptions.BrowserExecutableLocation = executableLocation;
                    firefoxOptions.AddArguments(new string[]
                    {
                        "--headless"
                    });
                    JGTest.factory = new FirefoxDriverFactory(workingDirectory, firefoxOptions, TimeSpan.FromSeconds(commandTimeout));
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
                    // NOTE: using a single instance of the ChromeDriver does not seem to be working correctly with 2.40.0
                    //var chromeService = ChromeDriverService.CreateDefaultService(workingDirectory);
                    //chromeService.Start();
                    //JGTest.service = chromeService;
                    JGTest.factory = new ChromeDriverFactory(workingDirectory, chromeOptions, TimeSpan.FromSeconds(commandTimeout));
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
            //if(service !=  null) service.Dispose();
        }
    }
}
