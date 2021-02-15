using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Crawler.Pages
{
    public static class WebDriverFactory
    {
        public static IWebDriver CreateWebDriver(string pathDriver, bool headless = false)
        {
            IWebDriver webDriver = null;

            ChromeOptions options = new ChromeOptions();
            if (headless)
                options.AddArgument("--headless");

            options.AddArgument("--no-sandbox");
            options.AddArgument("--allowed-ips=0.0.0.0");

            webDriver = new ChromeDriver(pathDriver, options);

            return webDriver;
        }
    }
}