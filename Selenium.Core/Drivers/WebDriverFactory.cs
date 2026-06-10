using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using Selenium.Core.Models;

namespace Selenium.Core.Drivers
{
    public interface IWebDriverFactory
    {
        IWebDriver InitializeDriver();
    }

    public class WebDriverFactory : IWebDriverFactory
    {
        private readonly TestSettings _settings;

        public WebDriverFactory(TestSettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public IWebDriver InitializeDriver()
        {
            var sel = _settings.Selenium;
            var browserName = sel!.Browser!.ToLower();
            IWebDriver driver;

            if (browserName == "chrome" || browserName == "chromium")
            {
                var options = new ChromeOptions();

                options.AddArguments(
                    "--disable-renderer-backgrounding",
                    "--disable-background-timer-throttling",
                    "--disable-backgrounding-occluded-windows",
                    "--disable-features=PaintHolding",
                    "--no-sandbox",
                    "--disable-dev-shm-usage",
                    "--mute-audio"
                );

                if (sel.Headless)
                {
                    options.AddArgument("--headless=new");
                    options.AddArgument("--disable-gpu");
                }

                var devicePreset = _settings.ActiveDevicePreset;

                if (!string.IsNullOrEmpty(devicePreset))
                {
                    options.EnableMobileEmulation(devicePreset);
                    TestContext.Progress.WriteLine($"Mobile emulation enabled: {devicePreset}, headless={sel.Headless}");
                }
                else
                {
                    TestContext.Progress.WriteLine($"Desktop browser launched: {browserName}, headless={sel.Headless}");
                }

                driver = new ChromeDriver(options);
            }
            else if (browserName == "firefox")
            {
                var options = new FirefoxOptions();
                if (sel.Headless) options.AddArgument("--headless");

                TestContext.Progress.WriteLine($"Desktop browser launched: Firefox, headless={sel.Headless}");
                driver = new FirefoxDriver(options);
            }
            else
            {
                throw new NotSupportedException($"Browser {browserName} is not supported.");
            }

            var activeDevice = _settings.ActiveDevicePreset;
            if (string.IsNullOrEmpty(activeDevice))
            {
                driver.Manage().Window.Size = new System.Drawing.Size(sel.WindowWidth, sel.WindowHeight);
            }

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(sel.DefaultTimeoutSeconds);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(sel.PageLoadTimeoutSeconds);

            return driver;
        }
    }
}