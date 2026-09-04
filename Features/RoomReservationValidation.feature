@ui
Feature: Room reservation validation
    As a guest
    I want clear feedback when required details are missing
    So that I can correct the reservation form

Scenario: Required guest details are validated
    Given the guest opens the Shady Meadows booking site
    When the guest searches for the displayed one-night stay
    And opens the first available room
    And starts the reservation
    And submits the guest details form without entering details
    Then booking validation errors should be displayed