using Reqnroll;
using Selenium.Reqnroll.Helpers;
using Selenium.Reqnroll.Models;
using Selenium.Reqnroll.Pages;

namespace Selenium.Reqnroll.Steps
{
    [Binding]
    public class PIMSteps
    {
        private readonly IPIMPage _pimPage;
        private readonly IActionHelpers _actionHelpers; 
        private readonly IAssertionHelpers _assertionHelpers;
        private Employee? _employee;

        public PIMSteps(
            IPIMPage pimPage,
            IActionHelpers actionHelpers,
            IAssertionHelpers assertionHelpers)
        {
            _pimPage = pimPage ?? throw new ArgumentNullException(nameof(pimPage));
            _actionHelpers = actionHelpers ?? throw new ArgumentNullException(nameof(actionHelpers));
            _assertionHelpers = assertionHelpers ?? throw new ArgumentNullException(nameof(assertionHelpers));
        }

        [When(@"I open the PIM ""([^""]+)"" section")]
        public void WhenIOpenPIMSection(string section)
        {
            _actionHelpers.OpenSection(section);
        }

        [When("I add a new employee")]
        public void WhenIAddANewEmployee()
        {
            _employee = new Employee();

            _actionHelpers.Type(_actionHelpers.Input("First Name", hasLabel: false), _employee.FirstName);
            _actionHelpers.Type(_actionHelpers.Input("Middle Name", hasLabel: false), _employee.MiddleName);
            _actionHelpers.Type(_actionHelpers.Input("Last Name", hasLabel: false), _employee.LastName);
            _actionHelpers.Type(_actionHelpers.Input("Employee Id", hasLabel: true), _employee.EmployeeId);

            _pimPage.SaveEmployee();
        }

        [When("I create a temporary employee")]
        public void WhenICreateATemporaryEmployee()
        {
            _employee = new Employee();

            _actionHelpers.OpenSection("Add Employee");
            _actionHelpers.Type(_actionHelpers.Input("First Name", hasLabel: false), _employee.FirstName);
            _actionHelpers.Type(_actionHelpers.Input("Last Name", hasLabel: false), _employee.LastName);
            _actionHelpers.Type(_actionHelpers.Input("Employee Id", hasLabel: true), _employee.EmployeeId);

            _pimPage.SaveEmployee();
        }

        [When("I search for that employee by ID")]
        public void WhenISearchForThatEmployeeById()
        {
            _actionHelpers.Type(_actionHelpers.Input("Employee Id", hasLabel: true), _employee!.EmployeeId);
            _actionHelpers.ButtonClick("Search");

            _assertionHelpers.AssertRecordsFoundHeader(1);
        }

        [When("I edit the employee's contact details")]
        public void WhenIEditTheEmployeesContactDetails()
        {
            _actionHelpers.ClickPencilIcon(1);
            _pimPage.OpenEmployeeSection("Contact Details");

            var newEmail = $"test_{Guid.NewGuid():N}@example.com";
            _actionHelpers.Type(_actionHelpers.Input("Work Email", hasLabel: true), newEmail);
            _actionHelpers.ButtonClick("Save");
        }

        [When("I delete that employee")]
        public void WhenIDeleteThatEmployee()
        {
            _actionHelpers.ClickDeleteIcon(1);
            _actionHelpers.ConfirmationButtonClick("Yes, Delete");
        }

        [When("I start deleting the employee but cancel the confirmation")]
        public void WhenICancelEmployeeDeletion()
        {
            _actionHelpers.ClickDeleteIcon(1);
            _actionHelpers.ConfirmationButtonClick("No, Cancel");
        }

        [Then("I should still find that employee in the list")]
        public void ThenIShouldStillFindEmployee()
        {
            _assertionHelpers.AssertRecordsFoundHeader(1);
        }
    }
}