Feature: Recruitment management
  As an HR administrator
  I want to manage candidates
  So that I can track recruitment activities efficiently

  Background:
    Given I am logged in as admin
    And I am on the "Recruitment" section

  Scenario: Create a new candidate
    When I open the Recruitment "Candidates" section
    And I add a new candidate
    Then the saved candidate details should match the input profile

  Scenario: Search returns no results for invalid keywords
    Given I create two temporary candidates
    And I am on the "Recruitment" section
    When I search for candidates by an invalid keyword
    Then I should see the "No Records Found" message

  Scenario: Search returns a single matching record for valid keywords
    Given I create two temporary candidates
    And I am on the "Recruitment" section
    When I search for candidates by the first candidate's keyword
    Then I should see exactly one matching record

  Scenario: Delete a candidate
    Given I create a temporary candidate
    And I am on the "Recruitment" section
    When I search for that candidate by keyword
    And I delete that candidate
    Then I should see the "Successfully Deleted" message
    And I should see the "No Records Found" message

