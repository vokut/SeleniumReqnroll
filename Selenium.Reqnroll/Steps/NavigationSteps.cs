using Reqnroll;
using Selenium.Reqnroll.Enums;
using Selenium.Reqnroll.Pages;

namespace Selenium.Reqnroll.Steps
{
    [Binding]
    public class NavigationSteps
    {
        private readonly INavigationPage _navigationPage;

        public NavigationSteps(INavigationPage navigationPage)
        {
            _navigationPage = navigationPage ?? throw new ArgumentNullException(nameof(navigationPage));
        }

        [When(@"I navigate to the ""(.*)"" section")]
        public void WhenINavigateToTheSection(MenuItems menuItem)
        {

            _navigationPage.ClickMenuItem(menuItem);
        }
    }
}