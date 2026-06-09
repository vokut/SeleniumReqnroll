using OpenQA.Selenium;
using Selenium.Reqnroll.Enums;
using Selenium.Reqnroll.Helpers;

namespace Selenium.Reqnroll.Pages
{
    public class NavigationPage : INavigationPage
    {
        private readonly IActionHelpers _actionHelpers;
        private readonly IWaitHelpers _waitHelpers;

        public NavigationPage(IActionHelpers actionHelpers, IWaitHelpers waitHelpers)
        {
            _actionHelpers = actionHelpers ?? throw new ArgumentNullException(nameof(actionHelpers));
            _waitHelpers = waitHelpers ?? throw new ArgumentNullException(nameof(waitHelpers));
        }

        private By GetMenuItemLocator(MenuItems item)
            => By.XPath($"//a[contains(@class, 'oxd-main-menu-item') and .//span[.='{item}']] | //a[.//span[.='{item}']]");

        public void ClickMenuItem(MenuItems item)
        {
            _actionHelpers.Click(GetMenuItemLocator(item));
        }

        public void WaitForNavigationMenuToLoad()
        {
            _waitHelpers.WaitForElementVisible(GetMenuItemLocator(MenuItems.Admin));
        }
    }

    public interface INavigationPage
    {
        void ClickMenuItem(MenuItems item);
        void WaitForNavigationMenuToLoad();
    }
}