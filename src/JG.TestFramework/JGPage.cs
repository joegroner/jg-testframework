using System;
using OpenQA.Selenium;

namespace JG.TestFramework
{
    public class JGPage
    {
        private readonly IWebDriver driver;
        private readonly Uri baseUrl;
        private readonly Uri pageUrl;

        public Uri BaseUrl
        {
            get { return this.baseUrl; }
        }

        public Uri PageUrl
        {
            get { return this.pageUrl; }
        }

        protected IWebDriver Driver
        {
            get { return this.driver; }
        }

        public JGPage(IWebDriver driver, Uri baseUrl, string path)
        {
            string pathVal;

            this.driver = driver ?? throw new ArgumentNullException(nameof(driver));
            this.baseUrl = baseUrl ?? throw new ArgumentNullException(nameof(baseUrl));
            pathVal = path ?? throw new ArgumentNullException(nameof(path));

            var builder = new UriBuilder(baseUrl);
            builder.Path = pathVal == string.Empty ? "/" : path;
            this.pageUrl = builder.Uri;
        }

        public virtual void Navigate()
        {
            this.driver.Navigate().GoToUrl(this.pageUrl);
        }
    }
}
