using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using Selenium.Core.Drivers;
using Selenium.Core.Models;

namespace Selenium.Reqnroll.Hooks
{
    [Binding]
    public class SeleniumHooks
    {
        private readonly IDriverManager _driverManager;
        private readonly TestSettings _config;

        public SeleniumHooks(IDriverManager driverManager, TestSettings config)
        {
            _driverManager = driverManager;
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        [BeforeScenario(Order = 1)] // Order = 1 ensures this runs AFTER DependenciesConfiguration (Order 0)
        public void BeforeScenario()
        {
            var factory = new WebDriverFactory(_config);
            IWebDriver driver = factory.InitializeDriver();

            _driverManager.SetDriver(driver);
        }

        [AfterScenario]
        public void AfterScenario()
        {
            try
            {
                // 🔹 Safely tear down using the instance reference
                _driverManager.QuitDriver();
            }
            catch (Exception ex)
            {
                TestContext.Progress.WriteLine($"Error during browser teardown: {ex.Message}");
            }
        }
    }
}