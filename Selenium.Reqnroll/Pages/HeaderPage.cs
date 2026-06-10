using OpenQA.Selenium;
using Selenium.Reqnroll.Helpers;

namespace Selenium.Reqnroll.Pages
{
    public class HeaderPage : IHeaderPage
    {
        public HeaderPage() { }

        public By BreadcrumbModule(string moduleName)
            => By.XPath($"//h6[contains(@class, 'oxd-topbar-header-breadcrumb-module') and normalize-space(.)='{moduleName}']");
    }

    public interface IHeaderPage
    {
        By BreadcrumbModule(string moduleName);
    }
}
