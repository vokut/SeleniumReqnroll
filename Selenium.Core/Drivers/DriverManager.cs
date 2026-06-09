using OpenQA.Selenium;

namespace Selenium.Core.Drivers
{
    public class DriverManager : IDriverManager
    {
        private IWebDriver? _driver;

        public IWebDriver Current => _driver
            ?? throw new NullReferenceException("WebDriver has not been initialized for this scenario.");

        public void SetDriver(IWebDriver driver)
        {
            _driver = driver;
        }

        public void QuitDriver()
        {
            if (_driver != null)
            {
                _driver.Quit();
                _driver.Dispose();
                _driver = null;
            }
        }
    }
    public interface IDriverManager
    {
        IWebDriver Current { get; }
        void SetDriver(IWebDriver driver);
        void QuitDriver();
    }
}