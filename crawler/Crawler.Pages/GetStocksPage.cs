using OpenQA.Selenium;
using System;
using System.IO;
using OpenQA.Selenium.Support.UI;
using Crawler.Pages;
using System.Collections.Generic;
using System.Globalization;

namespace AutoTest.Pages
{
    public class GetStocksPage
    {
        private IWebDriver _driver;

        public GetStocksPage()
        {
            this._driver = WebDriverFactory.CreateWebDriver((string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEB_DRIVER_DIRECTORY")) ? "C:\\webdriver" : Environment.GetEnvironmentVariable("WEB_DRIVER_DIRECTORY")), true);
        }

        public void Fechar()
        {
            _driver.Close();
        }

        public void CarregarPagina()
        {
            _driver.LoadPage(TimeSpan.FromSeconds(5), (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PAGE_URL"))) ? "http://localhost:5000/" : Environment.GetEnvironmentVariable("PAGE_URL"));
        }

        public void Screenshot()
        {
            ITakesScreenshot screenshot = _driver as ITakesScreenshot;
            var foto = screenshot.GetScreenshot();
            foto.SaveAsFile((string.IsNullOrEmpty(Environment.GetEnvironmentVariable("IMAGES_DIRECTORY"))) ? Path.Combine("C:\\images", $"IMG_{DateTime.Now.ToString("_MMddyyyy_HHmmss")}.png") : Path.Combine(Environment.GetEnvironmentVariable("IMAGES_DIRECTORY"), $"IMG_{DateTime.Now.ToString("_MMddyyyy_HHmmss")}.png"), ScreenshotImageFormat.Png);
        }

        public IEnumerable<Stock> GetStocks()
        {
            var tbody = this._driver.FindElement(By.TagName("tbody"));
            var rows = tbody.FindElements(By.TagName("tr"));

            IList<Stock> stocks = new List<Stock>();

            foreach (var row in rows)
            {
                var collumns = row.FindElements(By.TagName("td"));
                var value = collumns[1].Text;
                stocks.Add(new Stock() { Company = collumns[0].Text, Value = Convert.ToDouble(collumns[1].Text, new CultureInfo("en-US")) });
            }

            return stocks;
        }
    }

    public class Stock
    {
        public string Company { get; set; }
        public double Value { get; set; }
    }
}
