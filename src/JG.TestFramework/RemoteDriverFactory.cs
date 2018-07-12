using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Text;

namespace JG.TestFramework
{
    public class RemoteDriverFactory : IWebDriverFactory
    {
        private readonly Uri host;
        private readonly ICapabilities capabilities;
        private readonly TimeSpan commandTimeout;

        public RemoteDriverFactory(Uri host, ICapabilities capabilities, TimeSpan commandTimeout)
        {
            this.host = host ?? throw new ArgumentNullException(nameof(host));
            this.capabilities = capabilities ?? throw new ArgumentNullException(nameof(capabilities));
            this.commandTimeout = commandTimeout;
        }

        public IWebDriver Create()
        {
            return new RemoteWebDriver(this.host, this.capabilities, this.commandTimeout);
        }
    }
}
