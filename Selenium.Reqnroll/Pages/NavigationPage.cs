using OpenQA.Selenium;
using Selenium.Core.Models;
using Selenium.Reqnroll.Enums;
using Selenium.Reqnroll.Helpers;

namespace Selenium.Reqnroll.Pages
{
    public class NavigationPage : INavigationPage
    {
        private readonly IActionHelpers _actionHelpers;
        private readonly IWaitHelpers _waitHelpers;
        private readonly TestSettings _settings;

        public NavigationPage(IActionHelpers actionHelpers, IWaitHelpers waitHelpers, TestSettings settings)
        {
            _actionHelpers = actionHelpers ?? throw new ArgumentNullException(nameof(actionHelpers));
            _waitHelpers = waitHelpers ?? throw new ArgumentNullException(nameof(waitHelpers));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        private By GetMenuItemLocator(MenuItems item)
            => By.XPath($"//a[contains(@class, 'oxd-main-menu-item') and .//span[.='{item}']] | //a[.//span[.='{item}']]");

        private By HamburgerMenu => By.CssSelector(".oxd-topbar-header-hamburger");

        public void ClickMenuItem(MenuItems item)
        {
            if (_settings.IsMobileEmulationEnabled)
            {
                ClickHamburgerMenu();
            }

            _actionHelpers.Click(GetMenuItemLocator(item));
        }

        public void ClickHamburgerMenu()
        {
            _actionHelpers.Click(HamburgerMenu);
        }

        public void WaitForNavigationMenuToLoad()
        {
            _waitHelpers.WaitForElementVisible(GetMenuItemLocator(MenuItems.Admin));
        }
    }

    public interface INavigationPage
    {
        void ClickMenuItem(MenuItems item);
        void ClickHamburgerMenu();
        void WaitForNavigationMenuToLoad();
    }
}