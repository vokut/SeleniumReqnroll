using NUnit.Framework;
using OpenQA.Selenium;
using Selenium.Reqnroll.Enums;
using Selenium.Reqnroll.Helpers;
using Selenium.Reqnroll.Models;

namespace Selenium.Reqnroll.Pages
{
    public class PIMPage : IPIMPage
    {
        private readonly IActionHelpers _actionHelpers;
        private readonly IWaitHelpers _waitHelpers;
        private readonly IAssertionHelpers _assertionHelpers;

        public PIMPage(
            IActionHelpers actionHelpers,
            IWaitHelpers waitHelpers,
            IAssertionHelpers assertionHelpers)
        {
            _actionHelpers = actionHelpers ?? throw new ArgumentNullException(nameof(actionHelpers));
            _waitHelpers = waitHelpers ?? throw new ArgumentNullException(nameof(waitHelpers));
            _assertionHelpers = assertionHelpers ?? throw new ArgumentNullException(nameof(assertionHelpers));
        }

        private By EmployeeProfileContainer(string sectionName)
        {
            return By.XPath($"//div[contains(@class, 'orangehrm-card-container')][.//h6[normalize-space(.)='{sectionName}']]");
        }

        public void SaveEmployee()
        {
            _actionHelpers.ButtonClick("Save");

            _assertionHelpers.AssertPopupMessage(PopupMessages.SuccessfullySaved);

            _waitHelpers.WaitForElementVisible(EmployeeProfileContainer("Personal Details"), timeoutSeconds: 10);
        }

        public void AssertSavedEmployeeDetails(Employee employee)
        {
            _waitHelpers.WaitForElementAttributeValue(_actionHelpers.Input("First Name", hasLabel: false), "value", employee.FirstName);

            string actualFirstName = _waitHelpers.WaitForElementVisible(_actionHelpers.Input("First Name", hasLabel: false)).GetAttribute("value") ?? string.Empty;
            Assert.That(actualFirstName, Is.EqualTo(employee.FirstName));

            string actualMiddleName = _waitHelpers.WaitForElementVisible(_actionHelpers.Input("Middle Name", hasLabel: false)).GetAttribute("value") ?? string.Empty;
            Assert.That(actualMiddleName, Is.EqualTo(employee.MiddleName));

            string actualLastName = _waitHelpers.WaitForElementVisible(_actionHelpers.Input("Last Name", hasLabel: false)).GetAttribute("value") ?? string.Empty;
            Assert.That(actualLastName, Is.EqualTo(employee.LastName));

            string actualEmployeeId = _waitHelpers.WaitForElementVisible(_actionHelpers.Input("Employee Id", hasLabel: true)).GetAttribute("value") ?? string.Empty;
            Assert.That(actualEmployeeId, Is.EqualTo(employee.EmployeeId));
        }
    }
    
    public interface IPIMPage
    {
        void SaveEmployee();
        void AssertSavedEmployeeDetails(Employee employee);
    }
}