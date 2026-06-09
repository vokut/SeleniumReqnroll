using NUnit.Framework;
using Reqnroll;
using Selenium.Reqnroll.Enums;
using Selenium.Reqnroll.Helpers;
using Selenium.Reqnroll.Pages;

namespace Selenium.Reqnroll.Steps
{
    [Binding]
    public class AdminSteps
    {
        private readonly IAdminPage _adminPage;
        private readonly IActionHelpers _actionHelpers;
        private readonly IAssertionHelpers _assertionHelpers; 
        private string? _jobTitle;
        private string? _locationName;

        public AdminSteps(
            IAdminPage adminPage,
            IActionHelpers actionHelpers,
            IAssertionHelpers assertionHelpers)
        {
            _adminPage = adminPage ?? throw new ArgumentNullException(nameof(adminPage));
            _actionHelpers = actionHelpers ?? throw new ArgumentNullException(nameof(actionHelpers));
            _assertionHelpers = assertionHelpers ?? throw new ArgumentNullException(nameof(assertionHelpers));
        }

        [When(@"I open the ""([^""]+)"" -> ""([^""]+)"" section")]
        public void WhenIOpenAdminSubsection(string main, string sub)
        {
            _adminPage.OpenAdminSection(main, sub);
        }

        [When("I add a new job title")]
        public void WhenIAddANewJobTitle()
        {
            var random = new Random();
            _jobTitle = $"Software Engineer{random.Next(10000, 99999)}";

            _actionHelpers.ButtonClick("Add");

            _actionHelpers.Type(_actionHelpers.Input("Job Title"), _jobTitle);
            _actionHelpers.Type(_actionHelpers.Input("Job Description"), "Responsible for developing software solutions.");

            _actionHelpers.ButtonClick("Save");
        }

        [When("I add a new location")]
        public void WhenIAddANewLocation()
        {
            var random = new Random();
            _locationName = $"Location{random.Next(10000, 99999)}";

            _actionHelpers.ButtonClick("Add");
            _actionHelpers.Type(_actionHelpers.Input("Name"), _locationName);
            _actionHelpers.SelectDropdownOption("Country", "United States");
            _actionHelpers.ButtonClick("Save");
        }

        [When("I create a temporary location")]
        public void WhenICreateATemporaryLocation()
        {
            var random = new Random();
            _locationName = $"Location{random.Next(10000, 99999)}";

            _actionHelpers.ButtonClick("Add");
            _actionHelpers.Type(_actionHelpers.Input("Name"), _locationName);
            _actionHelpers.SelectDropdownOption("Country", "United States");
            _actionHelpers.ButtonClick("Save");

            _assertionHelpers.AssertPopupMessage(PopupMessages.SuccessfullySaved);
        }

        [When("I delete that location")]
        public void WhenIDeleteThatLocation()
        {
            _actionHelpers.Type(_actionHelpers.Input("Name"), _locationName!);
            _actionHelpers.ButtonClick("Search");

            _assertionHelpers.AssertRecordsFoundHeader(1);

            var cellValue = _actionHelpers.GetCellValue(1, "Name");
            Assert.That(cellValue, Is.EqualTo(_locationName));

            _actionHelpers.ClickDeleteIcon(1);
            _actionHelpers.ConfirmationButtonClick("Yes, Delete");
        }
    }
}