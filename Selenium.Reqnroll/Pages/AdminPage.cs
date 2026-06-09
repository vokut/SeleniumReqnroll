using OpenQA.Selenium;
using Selenium.Reqnroll.Enums;
using Selenium.Reqnroll.Helpers;

namespace Selenium.Reqnroll.Pages
{
    public class AdminPage : IAdminPage
    {
        private readonly INavigationPage _navigationPage;
        private readonly IActionHelpers _actionHelpers;

        public AdminPage(IActionHelpers actionHelpers, INavigationPage navigationPage)
        {
            _actionHelpers = actionHelpers ?? throw new ArgumentNullException(nameof(actionHelpers));
            _navigationPage = navigationPage ?? throw new ArgumentNullException(nameof(navigationPage));
        }

        public void OpenAdminSection(string mainSection, string subSection)
        {
            _navigationPage.ClickMenuItem(MenuItems.Admin);

            By mainSectionLocator = By.XPath($"//div[contains(@class, 'oxd-topbar-body')]//li[contains(., '{mainSection}')]");
            _actionHelpers.Click(mainSectionLocator);

            By subSectionLocator = By.XPath($"//nav//a[normalize-space(.)='{subSection}'] | //header//a[normalize-space(.)='{subSection}'] | //a[normalize-space(.)='{subSection}']");
            _actionHelpers.Click(subSectionLocator);
        }
    }

    public interface IAdminPage
    {
        void OpenAdminSection(string mainSection, string subSection);
    }
}