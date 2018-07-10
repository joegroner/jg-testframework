using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace JG.TestFramework
{
    public class FirefoxDriverFactory : IWebDriverFactory
    {
        private readonly string driverDirectory;
        private readonly FirefoxOptions options;
        private readonly TimeSpan commandTimeout;

        public FirefoxDriverFactory(string driverDirectory, FirefoxOptions options, TimeSpan commandTimeout)
        {
            this.driverDirectory = driverDirectory ?? throw new ArgumentNullException(nameof(driverDirectory));
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.commandTimeout = commandTimeout;
        }

        public IWebDriver Create()
        {
            return new FirefoxDriver(this.driverDirectory, this.options, this.commandTimeout);
        }
    }
}
