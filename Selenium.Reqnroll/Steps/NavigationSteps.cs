using Reqnroll;
using Selenium.Reqnroll.Enums;
using Selenium.Reqnroll.Helpers;
using Selenium.Reqnroll.Pages;

namespace Selenium.Reqnroll.Steps
{
    [Binding]
    public class NavigationSteps
    {
        private readonly INavigationPage _navigationPage;
        private readonly IActionHelpers _actionHelpers;

        public NavigationSteps(INavigationPage navigationPage, IActionHelpers actionHelpers)
        {
            _navigationPage = navigationPage ?? throw new ArgumentNullException(nameof(navigationPage));
            _actionHelpers = actionHelpers ?? throw new ArgumentNullException(nameof(actionHelpers));
        }

        [Given(@"I am on the ""(.*)"" section")]
        public void WhenINavigateToTheSection(MenuItems menuItem)
        {
            _navigationPage.ClickMenuItem(menuItem);
        }

        [When(@"I open the ""([^""]+)"" -> ""([^""]+)"" section")]
        public void WhenIOpenMenuSection(MenuItems menuItem, string section)
        {
            _navigationPage.ClickMenuItem(menuItem);
            _actionHelpers.OpenSection(section);
        }

        [When(@"I open the ""([^""]+)"" -> ""([^""]+)"" -> ""([^""]+)"" section")]
        public void WhenIOpenMenuSectionWithSub(MenuItems menuItem, string mainSection, string subSection)
        {
            _navigationPage.ClickMenuItem(menuItem);
            _actionHelpers.OpenSection(mainSection, subSection);
        }
    }
}