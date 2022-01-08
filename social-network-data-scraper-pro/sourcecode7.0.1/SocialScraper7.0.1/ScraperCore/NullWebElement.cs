using OpenQA.Selenium;


namespace ScraperCore
{
    public class NullWebElement : WebElement
    {
        public NullWebElement(IWebDriver parentDriver, string id) : base((WebDriver)parentDriver, id)
        { }

        public override string Text => string.Empty;

        public override bool Displayed => false;

        public override string GetAttribute(string attributeName)
        {
            return string.Empty;
        }

    }
}
