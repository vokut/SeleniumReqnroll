using NUnit.Framework;
using Reqnroll;
using Selenium.Reqnroll.Helpers;
using Selenium.Reqnroll.Models;
using Selenium.Reqnroll.Pages;

namespace Selenium.Reqnroll.Steps
{
    [Binding]
    public class RecruitmentSteps
    {
        private readonly IRecruitmentPage _recruitmentPage;
        private readonly IActionHelpers _actionHelpers;      
        private readonly IAssertionHelpers _assertionHelpers; 
        private Candidate? _candidate1;
        private Candidate? _candidate2;

        public RecruitmentSteps(
            IRecruitmentPage recruitmentPage,
            IActionHelpers actionHelpers,
            IAssertionHelpers assertionHelpers)
        {
            _recruitmentPage = recruitmentPage ?? throw new ArgumentNullException(nameof(recruitmentPage));
            _actionHelpers = actionHelpers ?? throw new ArgumentNullException(nameof(actionHelpers));
            _assertionHelpers = assertionHelpers ?? throw new ArgumentNullException(nameof(assertionHelpers));
        }

        [When(@"I open the Recruitment ""([^""]+)"" section")]
        public void WhenIOpenRecruitmentSection(string section)
        {
            _actionHelpers.OpenSection(section);
        }

        [When("I add a new candidate")]
        public void WhenIAddANewCandidate()
        {
            _candidate1 = new Candidate();

            _actionHelpers.ButtonClick("Add");
            _actionHelpers.Type(_actionHelpers.Input("First Name", hasLabel: false), _candidate1.FirstName);
            _actionHelpers.Type(_actionHelpers.Input("Middle Name", hasLabel: false), _candidate1.MiddleName);
            _actionHelpers.Type(_actionHelpers.Input("Last Name", hasLabel: false), _candidate1.LastName);
            _actionHelpers.Type(_actionHelpers.Input("Email", hasLabel: true), _candidate1.Email);
            _actionHelpers.Type(_actionHelpers.Input("Contact Number", hasLabel: true), _candidate1.PhoneNumber);
            _actionHelpers.Type(_actionHelpers.Input("Keywords", hasLabel: true), _candidate1.Keywords);

            _recruitmentPage.SaveCandidate();
        }

        [Then(@"the saved candidate details should match the input profile")]
        public void ThenTheSavedCandidateDetailsShouldMatchTheInputProfile()
        {
            _recruitmentPage.AssertSavedCandidateDetails(_candidate1!);
        }

        [Given("I create two temporary candidates")]
        public void WhenICreateTwoCandidates()
        {
            _candidate1 = new Candidate();
            _candidate2 = new Candidate();

            // Candidate 1
            _actionHelpers.OpenSection("Candidates");
            _actionHelpers.ButtonClick("Add");
            _actionHelpers.Type(_actionHelpers.Input("First Name", hasLabel: false), _candidate1.FirstName);
            _actionHelpers.Type(_actionHelpers.Input("Last Name", hasLabel: false), _candidate1.LastName);
            _actionHelpers.Type(_actionHelpers.Input("Email", hasLabel: true), _candidate1.Email);
            _actionHelpers.Type(_actionHelpers.Input("Keywords", hasLabel: true), _candidate1.Keywords);
            _recruitmentPage.SaveCandidate();

            // Candidate 2
            _actionHelpers.OpenSection("Candidates");
            _actionHelpers.ButtonClick("Add");
            _actionHelpers.Type(_actionHelpers.Input("First Name", hasLabel: false), _candidate2.FirstName);
            _actionHelpers.Type(_actionHelpers.Input("Last Name", hasLabel: false), _candidate2.LastName);
            _actionHelpers.Type(_actionHelpers.Input("Email", hasLabel: true), _candidate2.Email);
            _actionHelpers.Type(_actionHelpers.Input("Keywords", hasLabel: true), _candidate2.Keywords);
            _recruitmentPage.SaveCandidate();
        }

        [When("I search for candidates by an invalid keyword")]
        public void WhenISearchForInvalidKeyword()
        {
            _actionHelpers.Type(_actionHelpers.Input("Keywords", hasLabel: true), Guid.NewGuid().ToString());
            _actionHelpers.ButtonClick("Search");
        }

        [When("I search for candidates by the first candidate's keyword")]
        public void WhenISearchByFirstCandidatesKeyword()
        {
            _actionHelpers.Type(_actionHelpers.Input("Keywords", hasLabel: true), _candidate1!.Keywords);
            _actionHelpers.ButtonClick("Search");
        }

        [Given("I create a temporary candidate")]
        public void WhenICreateATemporaryCandidate()
        {
            _candidate1 = new Candidate();

            _actionHelpers.OpenSection("Candidates");
            _actionHelpers.ButtonClick("Add");
            _actionHelpers.Type(_actionHelpers.Input("First Name", hasLabel: false), _candidate1.FirstName);
            _actionHelpers.Type(_actionHelpers.Input("Last Name", hasLabel: false), _candidate1.LastName);
            _actionHelpers.Type(_actionHelpers.Input("Email", hasLabel: true), _candidate1.Email);
            _actionHelpers.Type(_actionHelpers.Input("Keywords", hasLabel: true), _candidate1.Keywords);
            _recruitmentPage.SaveCandidate();
        }

        [When("I search for that candidate by keyword")]
        public void WhenISearchForCandidateByKeyword()
        {
            _actionHelpers.Type(_actionHelpers.Input("Keywords", hasLabel: true), _candidate1!.Keywords);
            _actionHelpers.ButtonClick("Search");

            _assertionHelpers.AssertRecordsFoundHeader(1);
        }

        [When("I delete that candidate")]
        public void WhenIDeleteThatCandidate()
        {
            _actionHelpers.ClickDeleteIcon(1);
            _actionHelpers.ConfirmationButtonClick("Yes, Delete");
        }

        [Then("I should see exactly one matching record")]
        public void ThenIShouldSeeOneRecord()
        {
            _assertionHelpers.AssertRecordsFoundHeader(1);

            var cellValue = _actionHelpers.GetCellValue(1, "Candidate");
            Assert.That(cellValue.Trim(), Is.EqualTo($"{_candidate1!.FirstName} {_candidate1.LastName}"));
        }
    }
}