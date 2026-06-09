using Reqnroll;
using Selenium.Reqnroll.Enums;
using Selenium.Reqnroll.Helpers;

namespace Selenium.Reqnroll.Steps
{
    [Binding]
    public class MessagesSteps
    {
        private readonly IAssertionHelpers _assertionHelpers;

        public MessagesSteps(IAssertionHelpers assertionHelpers)
        {
            _assertionHelpers = assertionHelpers ?? throw new ArgumentNullException(nameof(assertionHelpers));
        }

        [Then(@"I should see the ""([^""]+)"" message")]
        public void ThenIShouldSeeTheMessage(PopupMessages popupType)
        {
            _assertionHelpers.AssertPopupMessage(popupType);
        }
    }
}