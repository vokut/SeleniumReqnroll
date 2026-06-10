namespace Selenium.Core.Models
{
    public class TestSettings
    {
        public FrameworkSettings? Framework { get; set; }
        public SeleniumSettings? Selenium { get; set; }

        public string? ActiveDevicePreset
            => Environment.GetEnvironmentVariable("DEVICE_PRESET") ?? Selenium?.MobileEmulationDevice;

        public bool IsMobileEmulationEnabled
            => !string.IsNullOrEmpty(ActiveDevicePreset);
    }

    public class FrameworkSettings
    {
        public string? ORANGEHRM_URL { get; set; }
        public string? ORANGEHRM_ADMIN_USER { get; set; }
        public string? ORANGEHRM_ADMIN_PASSWORD { get; set; }
    }

    public class SeleniumSettings
    {
        public string? Browser { get; set; }
        public bool Headless { get; set; }
        public int DefaultTimeoutSeconds { get; set; }
        public int PageLoadTimeoutSeconds { get; set; }
        public int WindowWidth { get; set; }
        public int WindowHeight { get; set; }
        public string? MobileEmulationDevice { get; set; }
    }
}