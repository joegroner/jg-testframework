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
        private class WebDriverWrapper
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

            public void DisposeWrapped()
            {
                this.wrapped.Dispose();
                this.wrapped = null;
            }
        }

        private static IWebDriverFactory factory;
        private static DriverService service;
        private static Uri baseUrl;
        // private static ThreadLocal<WebDriverWrapper> driver = new ThreadLocal<WebDriverWrapper>(() => new WebDriverWrapper(factory));
        
            
        /// <summary>
        /// The dictionary property for storing the web driver in the TestContext.
        /// </summary>
        private const string WebDriverPropkey = "WEB_DRIVER";
        public IWebDriver Driver
        {
            get => (IWebDriver)TestContext.Properties[WebDriverPropkey];
            set => TestContext.Properties[WebDriverPropkey] = value;
        }

        public Uri BaseUrl
        {
            get
            {
                return JGTest.baseUrl;
            }
        }

        public TestContext TestContext { get; set; }

        [AssemblyInitialize]
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
                    });
                    JGTest.factory = new FirefoxDriverFactory(workingDirectory, firefoxOptions, TimeSpan.FromSeconds(commandTimeout));
                    break;
                case "firefox-headless":
                    var firefoxHeadlessOptions = new FirefoxOptions();
                    firefoxHeadlessOptions.BrowserExecutableLocation = executableLocation;
                    firefoxHeadlessOptions.AddArguments(new string[]
                    {
                        "--headless"
                    });
                    JGTest.factory = new FirefoxDriverFactory(workingDirectory, firefoxHeadlessOptions, TimeSpan.FromSeconds(commandTimeout));
                    break;
                case "chrome-headless":
                    var chromeHeadlessOptions = new ChromeOptions();
                    chromeHeadlessOptions.AddArguments(new string[]
                    {
                        "--no-sandbox",
                        "--headless",
                        "--disabgpu"
                    });
                    JGTest.factory = new ChromeDriverFactory(workingDirectory, chromeHeadlessOptions, TimeSpan.FromSeconds(commandTimeout));
                    break;
                case "chrome":
                    var chromeOptions = new ChromeOptions();
                    chromeOptions.AddArguments(new string[]
                    {
                        "--no-sandbox",
                        "--disabgpu"
                    });
                    JGTest.factory = new ChromeDriverFactory(workingDirectory, chromeOptions, TimeSpan.FromSeconds(commandTimeout));
                    break;
                default:
                    var defaultchromeHeadlessOptions = new ChromeOptions();
                    defaultchromeHeadlessOptions.AddArguments(new string[]
                    {
                        "--no-sandbox",
                        "--headless",
                        "--disable-gpu"
                    });
                    // NOTE: the "single driver" approach does not seem to work with ChromeDriver 2.40.0 on Linux
                    //var chromeService = ChromeDriverService.CreateDefaultService(workingDirectory);
                    //chromeService.Start();
                    //JGTest.service = chromeService;
                    JGTest.factory = new ChromeDriverFactory(workingDirectory, defaultchromeHeadlessOptions, TimeSpan.FromSeconds(commandTimeout));
                    break;
            }

            JGTest.baseUrl = new Uri(baseUrl);
        }

        [TestInitialize]
        public virtual void TestInitialize()
        {
            TestContext.Properties[WebDriverPropkey] = factory.Create();
        }

        [TestCleanup]
        public virtual void TestCleanup()
        {
            Driver.Dispose();
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            //driver.Dispose();
            //if(service !=  null) service.Dispose();
        }
    }
}
