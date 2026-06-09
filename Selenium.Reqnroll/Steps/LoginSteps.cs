using Reqnroll;
using Selenium.Reqnroll.Pages;

namespace Selenium.Reqnroll.Steps
{
    [Binding]
    public class LoginSteps
    {
        private readonly ILoginPage _loginPage;

        public LoginSteps(ILoginPage loginPage)
        {
            _loginPage = loginPage ?? throw new ArgumentNullException(nameof(loginPage));
        }

        [Given("I am logged in as admin")]
        public void GivenIAmLoggedInAsAdmin()
        {
            _loginPage.PerformLogin();
        }
    }
}