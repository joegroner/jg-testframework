using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace JG.TestFramework
{
    public class ChromeDriverFactory : IWebDriverFactory
    {
        private readonly string driverDirectory;
        private readonly ChromeOptions options;
        private readonly TimeSpan commandTimeout;

        public ChromeDriverFactory(string driverDirectory, ChromeOptions options, TimeSpan commandTimeout)
        {
            this.driverDirectory = driverDirectory ?? throw new ArgumentNullException(nameof(driverDirectory));
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.commandTimeout = commandTimeout;
        }

        public IWebDriver Create()
        {
            return new ChromeDriver(this.driverDirectory, this.options, this.commandTimeout);
        }
    }
}
