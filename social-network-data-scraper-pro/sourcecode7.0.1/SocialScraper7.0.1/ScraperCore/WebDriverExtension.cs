using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace ScraperCore
{
    public static class WebDriverExtension
    {

        public static IWebElement FindElementWait(this IWebDriver webDriver, By by, int timeOut = 5)
        {
            try
            {
                var wait = new WebDriverWait(webDriver, timeout: TimeSpan.FromSeconds(timeOut))
                {
                    PollingInterval = TimeSpan.FromSeconds(1),
                };
                wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
                var element = wait.Until(drv => drv.FindElement(by));
                return element;
            }
            catch (StaleElementReferenceException ex)
            {
                Console.WriteLine($"FindElementWait-StaleElementReferenceException---{ex.Message}");
                return new NullWebElement(webDriver, "xcvbnndddd");

            }
            catch (WebDriverTimeoutException ex)
            {
                Console.WriteLine($"s----{ex.Message}");
                return new NullWebElement(webDriver, "xcvbnndddd");
            }

        }
        public static ReadOnlyCollection<IWebElement> FindElementsWait(this IWebDriver webDriver, By by, int timeOut = 5)
        {
            try
            {
                var wait = new WebDriverWait(webDriver, timeout: TimeSpan.FromSeconds(timeOut))
                {
                    PollingInterval = TimeSpan.FromSeconds(1),
                };
                wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
                var elements = wait.Until(drv => drv.FindElements(by));
                return elements;
            }
            catch (StaleElementReferenceException ex)
            {
                Console.WriteLine($"list----{ex.Message}");
                return new ReadOnlyCollection<IWebElement>(new List<IWebElement>());

            }
            catch (WebDriverTimeoutException ex)
            {
                Console.WriteLine(ex.Message + "timeout");
                return new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
            }

        }

        public static void MoveToElementCatch(this IWebDriver webDriver, IWebElement element)
        {
            try
            {
                var actionProvider = new Actions(webDriver);
                actionProvider.MoveToElement(element).Build().Perform();
            }
            catch (StaleElementReferenceException ex)
            {
                Console.WriteLine($"move----{ex.Message}");

            }

        }

        public static void  MoveToElementAndClick(this IWebDriver webDriver, IWebElement element)
        {
            bool state = true;
            while (state)
            {
                try
                {
                    var actionProvider = new Actions(webDriver);
                    actionProvider.MoveToElement(element).Build().Perform();
                    SpinWait.SpinUntil(() => false, 1000);
                    element.Click();
                    state = false;
                }
                catch (StaleElementReferenceException ex)
                {
                    Console.WriteLine($"moveclick----{ex.Message}");
                    throw ex;

                }
                catch (NoSuchElementException ex)
                {
                    Console.WriteLine($"moveclick----{ex.Message}");
                    throw ex;
                }

            }

        }


        public static IWebElement FindElementCatch(this IWebDriver webDriver, By by)
        {
            try
            {
                return webDriver.FindElement(by);
         
            }
            catch (NoSuchElementException ex)
            {
                return new NullWebElement(webDriver, "xcvbnndddd");
            }

        }
        public static void SelectByValue(this IWebDriver webDriver, By by, string selectValue)
        {
            var cateElement = webDriver.FindElement(by);
            var selectElement = new SelectElement(cateElement);
            selectElement.SelectByValue(selectValue);
        }

        public static void Search(this IWebDriver webDriver, By bySearch, string keyword)
        {
            var searchTexBox = webDriver.FindElementWait(bySearch);
            searchTexBox.Clear();
            searchTexBox.SendKeys(keyword + Keys.Enter);
        }
    }


}
