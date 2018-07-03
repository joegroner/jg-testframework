using System;
using OpenQA.Selenium;

namespace JG.TestFramework
{
    public interface IWebDriverFactory
    {
        IWebDriver Create();
    }
}
