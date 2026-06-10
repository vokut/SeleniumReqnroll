using OpenQA.Selenium;
using Selenium.Core.Drivers;
using Selenium.Core.Models;

namespace Selenium.Reqnroll.Helpers
{
    public class ActionHelpers : IActionHelpers
    {
        private readonly IWaitHelpers _wait;
        private readonly IDriverManager _driverManager;
        private readonly TestSettings _settings;

        public ActionHelpers(IWaitHelpers wait, IDriverManager driverManager, TestSettings settings)
        {
            _wait = wait ?? throw new ArgumentNullException(nameof(wait));
            _driverManager = driverManager ?? throw new ArgumentNullException(nameof(driverManager));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
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
            if (!_settings.IsMobileEmulationEnabled)
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
            else
            {
                By cellLocator = By.XPath($"((//div[contains(@class, 'oxd-card-table-body')]//div[contains(@class,'oxd-table-card --mobile')][{row}])//div[@class='header' and .='{columnName}'])/following-sibling::div");
                return GetText(cellLocator).Trim();
            }
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

        public void OpenSection(string mainSection, string? subSection = null)
        {
            var topBarNav = "//nav[contains(@class, 'oxd-topbar-body-nav') and @role='navigation']";
            var topBarItem = topBarNav + "//*[contains(@class, 'oxd-topbar-body-nav-tab-item') and normalize-space(.)='{0}']";
            By mainSectionLocator = By.XPath(string.Format(topBarItem, mainSection));
            By moreLocator = By.XPath(string.Format(topBarItem, "More"));
            By linkLocator = By.XPath($"//a[contains(@class, 'oxd-topbar-body-nav-tab-link') and normalize-space(.)='{mainSection}']");

            if (_settings.IsMobileEmulationEnabled)
            {
                _wait.WaitForElementVisible(By.XPath(topBarNav));

                if (_wait.IsElementVisible(mainSectionLocator))
                {
                    Click(mainSectionLocator);
                }
                else
                {
                    Click(moreLocator);
                    Click(linkLocator);
                }
            }
            else
            {
                Click(mainSectionLocator);
            }

            if (subSection != null)
            {
                By subSectionLocator = By.XPath($"//nav//a[normalize-space(.)='{subSection}'] | //header//a[normalize-space(.)='{subSection}'] | //a[normalize-space(.)='{subSection}']");
                Click(subSectionLocator);
            }
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

        public void OpenTabInItem(string tabName)
        {
            By tabLocator = By.XPath($"//a[contains(@class, 'orangehrm-tabs-item') and normalize-space(.)='{tabName}']");
            Click(tabLocator);
        }

        public void ExpandFilter()
        {
            By filterLocator = By.XPath("//div[@class='oxd-table-filter']");
            By filterIconLocator = By.XPath("//div[@class='oxd-table-filter']//i[contains(@class, 'oxd-icon bi-caret-down-fill')]");

            _wait.WaitForElementVisible(filterLocator);

            if (_wait.IsElementVisible(filterIconLocator))
            {
                Click(filterIconLocator);
            }
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
        void OpenSection(string mainSection, string? subSection = null);
        void ButtonClick(string buttonName);
        void ConfirmationButtonClick(string buttonName);
        void NavigateTo(string url);
        void OpenTabInItem(string tabName);
        void ExpandFilter();
    }
}