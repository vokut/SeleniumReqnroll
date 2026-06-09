using NUnit.Framework;
using OpenQA.Selenium;
using Selenium.Core.Models;
using Selenium.Reqnroll.Helpers;

namespace Selenium.Reqnroll.Pages
{
    public class LoginPage : ILoginPage
    {
        private readonly IActionHelpers _actionHelpers;
        private readonly IWaitHelpers _waitHelpers;
        private readonly TestSettings _config;

        public LoginPage(
            IActionHelpers actionHelpers,
            IWaitHelpers waitHelpers,
            TestSettings config)
        {
            _actionHelpers = actionHelpers ?? throw new ArgumentNullException(nameof(actionHelpers));
            _waitHelpers = waitHelpers ?? throw new ArgumentNullException(nameof(waitHelpers));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        private By UsernameInput => By.CssSelector("input[placeholder='Username']");
        private By PasswordInput => By.CssSelector("input[placeholder='Password']");
        private By LoginButton => By.CssSelector("button[type='submit'], button.oxd-button");
        private By VersionText => By.CssSelector(".orangehrm-copyright-wrapper p:first-of-type");
        private By BrandBanner => By.CssSelector(".oxd-brand-banner");

        public void GoToLoginPage()
        {
            _actionHelpers.NavigateTo(_config.Framework.ORANGEHRM_URL);
        }

        public void PerformLogin(bool navigate = true)
        {
            if (navigate)
                GoToLoginPage();

            _actionHelpers.Type(UsernameInput, _config.Framework.ORANGEHRM_ADMIN_USER);
            _actionHelpers.Type(PasswordInput, _config.Framework.ORANGEHRM_ADMIN_PASSWORD);
            _actionHelpers.Click(LoginButton);

            _waitHelpers.WaitForElementVisible(BrandBanner, timeoutSeconds: 15);
        }

        public void AssertVersion(string expectedVersion)
        {
            string actualVersion = _actionHelpers.GetText(VersionText);
            Assert.That(actualVersion, Is.EqualTo(expectedVersion));
        }
    }

    public interface ILoginPage
    {
        void GoToLoginPage();
        void PerformLogin(bool navigate = true);
        void AssertVersion(string expectedVersion);
    }
}