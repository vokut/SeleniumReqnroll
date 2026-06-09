using OpenQA.Selenium;
using Selenium.Reqnroll.Enums;

namespace Selenium.Reqnroll.Helpers
{
    public class AssertionHelpers : IAssertionHelpers
    {
        private readonly IWaitHelpers _wait;

        public AssertionHelpers(IWaitHelpers wait)
        {
            _wait = wait ?? throw new ArgumentNullException(nameof(wait));
        }

        public void AssertPopupMessage(PopupMessages popupMessage)
        {
            string messageText = popupMessage switch
            {
                PopupMessages.NoRecordsFound => "No Records Found",
                PopupMessages.SuccessfullyDeleted => "Successfully Deleted",
                PopupMessages.SuccessfullySaved => "Successfully Saved",
                PopupMessages.SuccessfullyUpdated => "Successfully Updated",
                _ => throw new ArgumentOutOfRangeException(nameof(popupMessage), popupMessage, null)
            };

            By toastLocator = By.XPath($"//div[contains(@class, 'oxd-toast')]//p[contains(., '{messageText}')]");

            _wait.WaitForElementVisible(toastLocator);

            _wait.WaitForElementInvisible(toastLocator, timeoutSeconds: 7);
        }

        public void AssertRecordsFoundHeader(int numberOfRecords)
        {
            var recordsText = $"({numberOfRecords}) Record{(numberOfRecords == 1 ? "" : "s")} Found";
            By locator = By.XPath($"//*[contains(@class, 'oxd-text') and contains(., '{recordsText}')]");

            _wait.WaitForElementVisible(locator);
        }
    }

    public interface IAssertionHelpers
    {
        void AssertPopupMessage(PopupMessages popupMessage);
        void AssertRecordsFoundHeader(int numberOfRecords);
    }
}