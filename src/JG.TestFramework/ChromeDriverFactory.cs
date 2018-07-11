using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace JG.TestFramework
{
    public class ChromeDriverFactory : IWebDriverFactory
    {
        private readonly ChromeDriverService service;
        private readonly ChromeOptions options;
        private readonly TimeSpan commandTimeout;

        public ChromeDriverFactory(ChromeDriverService service, ChromeOptions options, TimeSpan commandTimeout)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.commandTimeout = commandTimeout;
        }

        public IWebDriver Create()
        {
            return new ChromeDriver(this.service, this.options, this.commandTimeout);
        }
    }
}
