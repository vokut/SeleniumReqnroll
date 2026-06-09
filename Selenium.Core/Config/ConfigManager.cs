using dotenv.net;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Selenium.Core.Models;

namespace Selenium.Core.Config
{
    public static class ConfigManager
    {
        private static IConfigurationRoot? _config;
        private static string EnvironmentName { get; set; } = "dev";

        public static TestSettings Initialize()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);

            while (dir != null && !File.Exists(Path.Combine(dir.FullName, ".env")) && dir.GetFiles("*.sln").Length == 0)
            {
                dir = dir.Parent;
            }

            if (dir == null)
            {
                throw new DirectoryNotFoundException("Could not locate solution directory or .env file.");
            }

            var envPath = Path.Combine(dir.FullName, ".env");

            if (File.Exists(envPath))
            {
                DotEnv.Load(new DotEnvOptions(envFilePaths: [envPath], overwriteExistingVars: false));
            }

            EnvironmentName = Environment.GetEnvironmentVariable("TEST_ENVIRONMENT") ?? "dev";

            var builder = new ConfigurationBuilder()
                .SetBasePath(TestContext.CurrentContext.TestDirectory)
                .AddJsonFile("appsettings.json", false)
                .AddJsonFile($"appsettings.{EnvironmentName}.json", true)
                .AddEnvironmentVariables();

            _config = builder.Build();

            var settings = new TestSettings();
            _config.Bind(settings);
            TestContext.Progress.WriteLine($"Loaded config for {EnvironmentName}");

            return settings;
        }
    }
}