using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;

namespace ScraperCore
{
    public class ScraperBase
    {
        public bool IsRun { get; set; }
        public IWebDriver Driver { get; set; }

        private ChromeDriverService _driverService;
        private ChromeOptions _options;
        public ScraperBase()
        {
            _driverService = ChromeDriverService.CreateDefaultService();
            _driverService.HideCommandPromptWindow = true;
            _options = new ChromeOptions();
            //Hide Chrome is being controlled by automated software"
            _options.AddExcludedArgument("enable-automation");
            _options.AddArguments("--lang=en");
            _options.AddAdditionalOption("useAutomationExtension", false);
        }

        public void Start<T>(string url, Action<T> action = default, CancellationToken token = default)
        {

            if (!this.IsRun)
            {
                this.Driver = new ChromeDriver(_driverService, _options);
                this.Driver.Navigate().GoToUrl(url);
                this.IsRun = true;
            }
            else
            {
                try
                {
                     this.Driver.Navigate().GoToUrl(url);
                }
                catch (WebDriverException ex)
                {
                    if (ex.Message.Contains("target window already closed"))
                    {
                        this.Driver = new ChromeDriver(_driverService, _options);
                        this.Driver.Navigate().GoToUrl(url);
                    }
                }

            }
        }

        public void Quit()
        {
            if (this.Driver != null)
            {
                this.Driver.Quit();
            }
        }

        public void Close()
        {
            this.IsRun = false;
            if (this.Driver != null)
            {
                this.Driver.Close();
            }

        }
    }
}
