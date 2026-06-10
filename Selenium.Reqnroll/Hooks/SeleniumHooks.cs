using NUnit.Framework;
using Reqnroll;
using Selenium.Core.Drivers;

namespace Selenium.Reqnroll.Hooks
{
    [Binding]
    public class SeleniumHooks
    {
        private readonly IDriverManager _driverManager;
        private readonly IWebDriverFactory _driverFactory;

        public SeleniumHooks(IDriverManager driverManager, IWebDriverFactory driverFactory)
        {
            _driverManager = driverManager ?? throw new ArgumentNullException(nameof(driverManager));
            _driverFactory = driverFactory ?? throw new ArgumentNullException(nameof(driverFactory));
        }

        [BeforeScenario(Order = 1)]
        public void BeforeScenario()
        {
            _driverManager.SetDriver(_driverFactory.InitializeDriver());
        }

        [AfterScenario]
        public void AfterScenario()
        {
            try
            {
                _driverManager.QuitDriver();
            }
            catch (Exception ex)
            {
                TestContext.Progress.WriteLine($"Error during browser teardown: {ex.Message}");
            }
        }
    }
}