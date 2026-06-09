using OpenQA.Selenium;
using Selenium.Core.Drivers; 

namespace Selenium.Reqnroll.Helpers
{
    public class ActionHelpers : IActionHelpers
    {
        private readonly IWaitHelpers _wait;
        private readonly IDriverManager _driverManager;

        public ActionHelpers(IWaitHelpers wait, IDriverManager driverManager)
        {
            _wait = wait ?? throw new ArgumentNullException(nameof(wait));
            _driverManager = driverManager ?? throw new ArgumentNullException(nameof(driverManager));
        }

        public void Click(By locator, int? timeoutSeconds = null)
        {
            _wait.WaitForElementClickable(locator, timeoutSeconds).Click();
        }

        public void Type(By locator, string text, int? timeoutSeconds = null)
        {
            var element = _wait.WaitForElementVisible(locator, timeoutSeconds);
            element.Click();
            element.SendKeys(Keys.Control + "a");
            element.SendKeys(text);
        }

        public string GetText(By locator, int? timeoutSeconds = null)
        {
            return _wait.WaitForElementVisible(locator, timeoutSeconds).Text;
        }

        public void SelectDropdownOption(string label, string option)
        {
            By dropdownInput = By.XPath($"//div[contains(@class, 'oxd-input-field-bottom-space')][.//label[normalize-space(.)='{label}']]//div[contains(@class, 'oxd-select-text-input')]");
            Click(dropdownInput);

            By optionLocator = By.XPath($"//div[@role='listbox' or @role='option']//span[normalize-space(.)='{option}'] | //div[@role='option'][normalize-space(.)='{option}']");
            Click(optionLocator);
        }

        public By Input(string name, bool hasLabel = true)
        {
            if (!hasLabel)
            {
                return By.XPath($"//input[@placeholder='{name}'] | //textarea[@placeholder='{name}']");
            }

            return By.XPath($"//div[contains(@class, 'oxd-input-field-bottom-space')][.//label[normalize-space(.)='{name}']]//*[self::input or self::textarea]");
        }

        public string GetCellValue(int row, string columnName)
        {
            var headerCells = _wait.FindElements(By.CssSelector(".oxd-table-header .oxd-table-header-cell"));
            int headerIndex = -1;

            for (int i = 0; i < headerCells.Count; i++)
            {
                if (headerCells[i].Text.Trim() == columnName)
                {
                    headerIndex = i + 1;
                    break;
                }
            }

            if (headerIndex == -1)
                throw new Exception($"Column '{columnName}' not found.");

            By cellLocator = By.XPath($"//div[contains(@class, 'oxd-table-card')][{row}]//div[contains(@class, 'oxd-table-cell')][{headerIndex}]");
            return GetText(cellLocator).Trim();
        }

        public void ClickPencilIcon(int row)
        {
            By pencilLocator = By.XPath($"//div[contains(@class, 'oxd-table-body')]//div[contains(@class, 'oxd-table-row')][{row}]//i[contains(@class, 'bi-pencil-fill')]");
            Click(pencilLocator);
        }

        public void ClickDeleteIcon(int row)
        {
            By trashLocator = By.XPath($"//div[contains(@class, 'oxd-table-body')]//div[contains(@class, 'oxd-table-row')][{row}]//i[contains(@class, 'bi-trash')]");
            Click(trashLocator);
        }

        public void OpenSection(string sectionName)
        {
            By sectionLink = By.XPath($"//a[contains(@role, 'link') and normalize-space(.)='{sectionName}'] | //a[normalize-space(.)='{sectionName}']");
            Click(sectionLink);
        }

        public void ButtonClick(string buttonName)
        {
            By buttonLocator = By.XPath($"//button[normalize-space(.)='{buttonName}']");
            Click(buttonLocator);
        }

        public void ConfirmationButtonClick(string buttonName)
        {
            By confirmButton = By.XPath($"//button[normalize-space(.)='{buttonName}']");
            _wait.WaitForElementVisible(confirmButton);
            Click(confirmButton);
        }

        public void NavigateTo(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("URL cannot be null or empty.", nameof(url));

            _driverManager.Current.Navigate().GoToUrl(url);
        }
    }

    public interface IActionHelpers
    {
        void Click(By locator, int? timeoutSeconds = null);
        void Type(By locator, string text, int? timeoutSeconds = null);
        string GetText(By locator, int? timeoutSeconds = null);
        void SelectDropdownOption(string label, string option);
        By Input(string name, bool hasLabel = true);
        string GetCellValue(int row, string columnName);
        void ClickPencilIcon(int row);
        void ClickDeleteIcon(int row);
        void OpenSection(string sectionName);
        void ButtonClick(string buttonName);
        void ConfirmationButtonClick(string buttonName);
        void NavigateTo(string url); // 🔹 4. Добавен в интерфейсния контракт
    }
}