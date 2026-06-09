namespace Selenium.Core.Models
{
    public class TestSettings
    {
        public FrameworkSettings Framework { get; set; } = new();
        public SeleniumSettings Selenium { get; set; } = new();
    }

    public class FrameworkSettings
    {
        public string ORANGEHRM_URL { get; set; } = "";
        public string ORANGEHRM_ADMIN_USER { get; set; } = "";
        public string ORANGEHRM_ADMIN_PASSWORD { get; set; } = "";
    }

    public class SeleniumSettings
    {
        public string Browser { get; set; } = "chrome";
        public bool Headless { get; set; } = false;
        public int DefaultTimeoutSeconds { get; set; } = 30;
        public int PageLoadTimeoutSeconds { get; set; } = 30;
        public int WindowWidth { get; set; } = 1920;
        public int WindowHeight { get; set; } = 1080;
        public string MobileEmulationDevice { get; set; } = "";
    }
}