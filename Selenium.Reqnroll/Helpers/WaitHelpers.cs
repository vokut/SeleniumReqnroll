using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Selenium.Core.Drivers;

namespace Selenium.Reqnroll.Helpers
{
    public class WaitHelpers : IWaitHelpers
    {
        private readonly IDriverManager _driverManager;
        private readonly int _defaultTimeoutSeconds = 10;
        private readonly TimeSpan _originalImplicitWait;

        public WaitHelpers(IDriverManager driverManager)
        {
            _driverManager = driverManager ?? throw new ArgumentNullException(nameof(driverManager));

            _originalImplicitWait = Driver.Manage().Timeouts().ImplicitWait;
        }

        public IWebDriver Driver => _driverManager.Current ?? throw new NullReferenceException("WebDriver has not been initialized.");

        private WebDriverWait GetWait(int? timeoutSeconds)
        {
            int seconds = timeoutSeconds ?? _defaultTimeoutSeconds;
            return new WebDriverWait(Driver, TimeSpan.FromSeconds(seconds));
        }

        public IWebElement WaitForElementVisible(By locator, int? timeoutSeconds = null)
        {
            try
            {
                Driver.Manage().Timeouts().ImplicitWait = TimeSpan.Zero;

                return GetWait(timeoutSeconds).Until(d =>
                {
                    var elements = d.FindElements(locator);
                    if (elements.Count > 0 && elements[0].Displayed)
                    {
                        return elements[0];
                    }
                    return null!;
                });
            }
            catch (WebDriverTimeoutException)
            {
                Assert.Fail($"Assertion Failed: Expected element to be VISIBLE in {timeoutSeconds ?? _defaultTimeoutSeconds} seconds.\nLocator used: {locator}");
                return null!;
            }
            finally
            {
                Driver.Manage().Timeouts().ImplicitWait = _originalImplicitWait;
            }
        }

        public IWebElement WaitForElementClickable(By locator, int? timeoutSeconds = null)
        {
            try
            {
                Driver.Manage().Timeouts().ImplicitWait = TimeSpan.Zero;

                return GetWait(timeoutSeconds).Until(d =>
                {
                    var elements = d.FindElements(locator);

                    if (elements.Count > 0)
                    {
                        var element = elements[0];

                        if (element.Displayed && element.Enabled)
                        {
                            return element;
                        }
                    }

                    return null!;
                });
            }
            catch (WebDriverTimeoutException)
            {
                Assert.Fail($"Assertion Failed: Expected element to be CLICKABLE (Visible & Enabled) within {timeoutSeconds ?? _defaultTimeoutSeconds} seconds.\nLocator used: {locator}");
                return null!;
            }
            finally
            {
                Driver.Manage().Timeouts().ImplicitWait = _originalImplicitWait;
            }
        }

        public void WaitForElementInvisible(By locator, int? timeoutSeconds = null)
        {
            try
            {
                Driver.Manage().Timeouts().ImplicitWait = TimeSpan.Zero;

                GetWait(timeoutSeconds).Until(d =>
                {
                    var elements = d.FindElements(locator);

                    if (elements.Count == 0)
                        return true;

                    try
                    {
                        return !elements[0].Displayed;
                    }
                    catch (StaleElementReferenceException)
                    {
                        return true;
                    }
                });
            }
            catch (WebDriverTimeoutException)
            {
                Assert.Fail($"Assertion Failed: Expected element to be INVISIBLE/GONE within {timeoutSeconds ?? _defaultTimeoutSeconds} seconds.\nLocator used: {locator}");
            }
            finally
            {
                Driver.Manage().Timeouts().ImplicitWait = _originalImplicitWait;
            }
        }

        public void Wait(int seconds)
        {
            Thread.Sleep(seconds * 1000);
        }

        public IReadOnlyList<IWebElement> FindElements(By locator, int? timeoutSeconds = null)
        {
            return WaitForElementVisible(locator, timeoutSeconds) != null
                ? Driver.FindElements(locator)
                : new System.Collections.Generic.List<IWebElement>();
        }

        public void WaitForElementAttributeValue(By locator, string attributeName, string expectedValue, int? timeoutSeconds = null)
        {
            var element = WaitForElementVisible(locator, timeoutSeconds);

            try
            {
                GetWait(timeoutSeconds).Until(d =>
                {
                    var currentValue = element.GetAttribute(attributeName) ?? string.Empty;
                    return string.Equals(currentValue, expectedValue, StringComparison.OrdinalIgnoreCase);
                });
            }
            catch (WebDriverTimeoutException)
            {
                Assert.Fail($"Assertion Failed: Expected element attribute '{attributeName}' to be '{expectedValue}' within {timeoutSeconds ?? _defaultTimeoutSeconds} seconds.\nLocator used: {locator}");
            }
        }
        
        public bool IsElementVisible(By locator)
        {
            try
            {
                Driver.Manage().Timeouts().ImplicitWait = TimeSpan.Zero;
                var elements = Driver.FindElements(locator);
                return elements.Count > 0 && elements[0].Displayed;
            }
            finally
            {
                Driver.Manage().Timeouts().ImplicitWait = _originalImplicitWait;
            }
        }
    }

    public interface IWaitHelpers
    {
        IWebElement WaitForElementVisible(By locator, int? timeoutSeconds = null);
        IWebElement WaitForElementClickable(By locator, int? timeoutSeconds = null);
        void WaitForElementInvisible(By locator, int? timeoutSeconds = null);
        void Wait(int seconds);
        IReadOnlyList<IWebElement> FindElements(By locator, int? timeoutSeconds = null);
        void WaitForElementAttributeValue(By locator, string attributeName, string expectedValue, int? timeoutSeconds = null);
        bool IsElementVisible(By locator);
    }
}
