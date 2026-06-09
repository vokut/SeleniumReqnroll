using Microsoft.Extensions.DependencyInjection;
using Reqnroll.Microsoft.Extensions.DependencyInjection;
using Selenium.Core.Config;
using Selenium.Core.Drivers;
using Selenium.Core.Models;
using Selenium.Reqnroll.Helpers;
using Selenium.Reqnroll.Pages;

namespace Selenium.Reqnroll.Support
{
    public static class DependenciesConfiguration
    {
        [ScenarioDependencies]
        public static IServiceCollection CreateServices()
        {
            var services = new ServiceCollection();

            TestSettings config = ConfigManager.Initialize();
            services.AddSingleton<TestSettings>(config);

            services.AddScoped<IDriverManager, DriverManager>();

            services.AddScoped<IWaitHelpers, WaitHelpers>();
            services.AddScoped<IActionHelpers, ActionHelpers>();
            services.AddScoped<IAssertionHelpers, AssertionHelpers>();

            services.AddScoped<ILoginPage, LoginPage>();
            services.AddScoped<INavigationPage, NavigationPage>();
            services.AddScoped<IRecruitmentPage, RecruitmentPage>();
            services.AddScoped<IAdminPage, AdminPage>();
            services.AddScoped<IPIMPage, PIMPage>();

            return services;
        }
    }
}