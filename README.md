# Enterprise Test Automation Framework (Reqnroll + C#)

A modular, enterprise-grade BDD test automation framework built with **.NET 8**, **Reqnroll**, **Selenium WebDriver**, and **NUnit**. Designed with the Page Object Model (POM), Dependency Injection (DI), structured Serilog logging, and Allure failure-diagnostic reporting.

---

## 🛠 Tech Stack & Dependencies

| Category | Technology | Version | Purpose |
| :--- | :--- | :--- | :--- |
| **Runtime** | .NET SDK | 8.0 | Core execution environment |
| **BDD Engine** | Reqnroll.NUnit | 1.3.2 / 2.x | Gherkin scenario orchestration |
| **Test Framework** | NUnit | 4.1.0 | Test runner and assertions |
| **Browser Engine** | Selenium WebDriver | 4.23.0 | Web element interaction & control |
| **Reporting** | Allure.Reqnroll | 2.12.1 | Living documentation & failure analytics |
| **Logging** | Serilog | 4.0.0 | Structured rolling file and console logs |
| **Configuration** | Microsoft.Extensions.Configuration | 8.0.0 | JSON settings deserialization |

---

## 📂 Project Structure

```text
reqnroll_project/
├── Config/
│   ├── ConfigReader.cs            # Deserializes appsettings.json into strongly-typed objects
│   └── TestSettings.cs            # Configuration model (Browser, BaseUrl, Timeouts)
├── Features/
│   └── RoomReservationValidation.feature  # Gherkin scenario specifications
├── Logs/                          # Daily rolling test execution logs (.log)
├── Pages/
│   ├── BasePage.cs                # Core driver wrapper, explicit waits, SafeClick logic
│   ├── LoginPage.cs               # SauceDemo page object
│   └── RoomReservationPage.cs     # Shady Meadows booking page object
├── StepDefinitions/
│   └── RoomReservationValidationSteps.cs  # Reqnroll step bindings
├── Support/
│   ├── DriverFactory.cs           # Browser initialization (Chrome, Firefox, Edge)
│   ├── Hooks.cs                   # Scenario lifecycle, DI container setup, screenshots
│   └── TestInitializer.cs        # Pre-execution hooks (Allure folder scaffolding)
├── Utilities/
│   └── TestLogger.cs              # Thread-safe Serilog file and console logger
├── allureConfig.json              # Allure report engine configuration
├── appsettings.json               # Environment targets, browser choice, timeouts
└── reqnroll_project.csproj        # Build configuration and package references