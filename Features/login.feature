Feature: User Login

 Scenario: Successful login with valid credentials
 Given the user is on the login page
 When they enter a valid username and password
 Then they should be redirected to the dashboard