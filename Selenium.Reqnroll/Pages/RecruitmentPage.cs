using NUnit.Framework;
using OpenQA.Selenium;
using Selenium.Reqnroll.Helpers;
using Selenium.Reqnroll.Models;

namespace Selenium.Reqnroll.Pages
{
    public class RecruitmentPage : IRecruitmentPage
    {
        private readonly IActionHelpers _actionHelpers;
        private readonly IWaitHelpers _waitHelpers;

        public RecruitmentPage(IActionHelpers actionHelpers, IWaitHelpers waitHelpers)
        {
            _actionHelpers = actionHelpers ?? throw new ArgumentNullException(nameof(actionHelpers));
            _waitHelpers = waitHelpers ?? throw new ArgumentNullException(nameof(waitHelpers));
        }

        private By CandidateProfileContainer =>
            By.XPath("//div[contains(@class, 'orangehrm-card-container')][.//h6[normalize-space(.)='Candidate Profile']]");


        public void SaveCandidate()
        {
            _actionHelpers.ButtonClick("Save");

            _waitHelpers.WaitForElementVisible(CandidateProfileContainer, timeoutSeconds: 15);
        }

        public void AssertSavedCandidateDetails(Candidate candidate)
        {
            string actualFirstName = _waitHelpers.WaitForElementVisible(_actionHelpers.Input("First Name", hasLabel: false)).GetAttribute("value") ?? string.Empty;
            Assert.That(actualFirstName, Is.EqualTo(candidate.FirstName));

            string actualMiddleName = _waitHelpers.WaitForElementVisible(_actionHelpers.Input("Middle Name", hasLabel: false)).GetAttribute("value") ?? string.Empty;
            Assert.That(actualMiddleName, Is.EqualTo(candidate.MiddleName));

            string actualLastName = _waitHelpers.WaitForElementVisible(_actionHelpers.Input("Last Name", hasLabel: false)).GetAttribute("value") ?? string.Empty;
            Assert.That(actualLastName, Is.EqualTo(candidate.LastName));

            string actualEmail = _waitHelpers.WaitForElementVisible(_actionHelpers.Input("Email", hasLabel: true)).GetAttribute("value") ?? string.Empty;
            Assert.That(actualEmail, Is.EqualTo(candidate.Email));

            string actualContactNumber = _waitHelpers.WaitForElementVisible(_actionHelpers.Input("Contact Number", hasLabel: true)).GetAttribute("value") ?? string.Empty;
            Assert.That(actualContactNumber, Is.EqualTo(candidate.PhoneNumber));

            string actualKeywords = _waitHelpers.WaitForElementVisible(_actionHelpers.Input("Keywords", hasLabel: true)).GetAttribute("value") ?? string.Empty;
            Assert.That(actualKeywords, Is.EqualTo(candidate.Keywords));
        }
    }

    public interface IRecruitmentPage
    {
        void SaveCandidate();
        void AssertSavedCandidateDetails(Candidate candidate);
    }
}