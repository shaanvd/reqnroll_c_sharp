# Enterprise Test Automation Framework (Reqnroll + C#)

A modular, enterprise-grade, and fully dockerized BDD test automation framework built with **.NET 8**, **Reqnroll**, **Selenium WebDriver**, and **NUnit**. Designed with the Page Object Model (POM), BoDi Dependency Injection (DI), structured Serilog logging, Allure failure diagnostics, and containerized cross-platform orchestration via **Docker Compose**, **Selenium Grid**, and **noVNC**.

---

## 🛠 Tech Stack & Dependencies

| Category | Technology | Version | Purpose |
| :--- | :--- | :--- | :--- |
| **Runtime** | .NET SDK | 8.0 | Core execution and build environment |
| **BDD Engine** | Reqnroll.NUnit | 2.0.0 | Gherkin scenario parser & step binding execution |
| **Test Framework** | NUnit | 4.1.0 | Test runner, assertions, and lifecycle management |
| **Browser Engine** | Selenium WebDriver | 4.23.0 | Local & remote browser interaction via W3C protocol |
| **Infrastructure** | Docker & Docker Compose | Compose V2 | Containerization, Selenium Grid & noVNC execution |
| **Reporting** | Allure.Reqnroll | 2.12.1 | Living documentation, step analytics & screenshot capture |
| **Logging** | Serilog | 4.0.0 | Thread-safe, structured rolling file & console logging |
| **Configuration** | Microsoft.Extensions.Configuration | 8.0.0 | JSON deserialization & environment variable binding |

---

## 📂 Project Structure

    reqnroll_project/
    ├── Config/
    │   ├── ConfigReader.cs            # Loads appsettings.json and binds Docker env variables
    │   └── TestSettings.cs            # Configuration model (Grid settings, URLs, users, timeouts)
    ├── Features/
    │   ├── login.feature              # Authentication Gherkin specifications
    │   └── RoomReservationValidation.feature  # Booking engine scenario specifications
    ├── Logs/                          # Daily rolling test execution logs (.log)
    ├── Models/
    │   └── UserCredentials.cs         # Strongly typed model for user credentials
    ├── Pages/
    │   ├── BasePage.cs                # Core driver wrapper, explicit waits, SafeClick logic
    │   ├── LoginPage.cs               # SauceDemo page object
    │   └── RoomReservationPage.cs     # Shady Meadows booking page object
    ├── StepDefinitions/
    │   ├── LoginSteps.cs              # Authentication step bindings
    │   └── RoomReservationValidationSteps.cs  # Reservation step bindings
    ├── Support/
    │   ├── Hooks.cs                   # Scenario lifecycle, DI container registration, screenshots
    │   └── TestInitializer.cs         # Pre-execution hooks (folder and artifact setup)
    ├── Utilities/
    │   ├── DriverFactory.cs           # Local ChromeDriver & Selenium Grid RemoteWebDriver factory
    │   └── TestLogger.cs              # Thread-safe Serilog file and console logger
    ├── .dockerignore                  # Prevents host artifacts from copying into build context
    ├── allureConfig.json              # Allure report engine configuration
    ├── appsettings.json               # Default local configurations, base URLs, and credentials
    ├── docker-compose.yaml            # Multi-container orchestration (selenium-chrome + test-runner)
    ├── Dockerfile                     # Multi-stage .NET 8 test runner container specification
    └── reqnroll_project.csproj        # Build configuration, targets, and package dependencies

---

## ⚙️ Prerequisites

* [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* [Docker Desktop](https://www.docker.com/products/docker-desktop/) (with Linux containers enabled)
* [Google Chrome](https://www.google.com/chrome/) (for local runs outside Docker)
* [Java JRE/JDK 8+](https://www.java.com/) (required to generate Allure HTML reports locally)
* **Allure CLI**:

    npm install -g allure-commandline --save-dev
    # or via Scoop:
    scoop install allure

---

## 🚀 Configuration (appsettings.json)

The framework relies on `appsettings.json` for base settings, with native support for hierarchical environment variable overrides (`TestSettings__*`):

    {
      "TestSettings": {
        "BaseUrl": "https://www.saucedemo.com/",
        "ShadyMeadowsUrl": "https://automationintesting.online/",
        "Browser": "Chrome",
        "TimeoutSeconds": 10,
        "LogDirectory": "TestResults/Logs",
        "UseGrid": false,
        "GridUrl": "http://localhost:4444/wd/hub",
        "Users": {
          "StandardUser": {
            "Username": "standard_user",
            "Password": "secret_sauce"
          },
          "LockedOutUser": {
            "Username": "locked_out_user",
            "Password": "secret_sauce"
          }
        }
      }
    }

---

## 🧪 Execution Modes

### Mode 1: Containerized Execution (Docker + Selenium Grid + noVNC)

The dual-container architecture decouples test orchestration from browser runtime dependencies:

    [test-runner (.NET 8)] ──HTTP (RemoteWebDriver)──> [selenium-chrome (Grid + Xvfb)] <──Browser [noVNC :7900]

#### 1. Full Automated Run (Headless / CI Mode)
Builds the runner, awaits Grid health status, runs scenarios, maps results back to host, and exits:

    docker compose up --build --abort-on-container-exit

#### 2. Interactive Live Debugging with noVNC
To watch test execution in real time inside a browser session:

1. **Spin up the Grid service in the background:**

    docker compose up -d selenium-chrome

2. **Open the noVNC Viewer:**  
   Navigate to **http://localhost:7900** in your host browser and click **Connect** (default password: `secret` if prompted; bypassed when `VNC_NO_PASSWORD=1`).
3. **Execute the test runner in an adjacent terminal:**

    docker compose run --rm test-runner

4. **Teardown containers:**

    docker compose down -v

---

### Mode 2: Local Execution (Visual Studio or CLI)

When `UseGrid: false` is active in `appsettings.json`, tests run directly against your local browser:

    # Run all tests via .NET CLI
    dotnet test

    # Run a specific feature or scenario
    dotnet test --filter "FullyQualifiedName~UserAuthenticationFeature"

---

## 📊 Viewing Reports & Artifacts

### 1. Interactive Allure Report
On test execution, raw JSON files and failure screenshots are output to `TestResults/` and mapped directly to your host workspace via Docker volumes:

    # Serve report directly to default browser
    allure serve bin/Debug/net8.0/TestResults

    # Or generate static HTML distribution
    allure generate bin/Debug/net8.0/TestResults -o allure-report --clean
    allure open allure-report

### 2. Failure Evidence & Screenshots
* Captured automatically in `Hooks.cs` via `ITakesScreenshot` whenever `_scenarioContext.TestError != null`.
* Attached directly to the failing step in the Allure timeline report as base64 PNG data.

### 3. Execution Logs (Serilog)
* Thread-safe, non-blocking logs write to both the live console and cross-platform rolling files:

    reqnroll_project/Logs/test_execution_YYYYMMDD.log

* Configured with `shared: true` to prevent NUnit test host file-lock contention across concurrent execution threads.

---

## 🛡 Design Patterns & Stability Safeguards

* **Remote / Local Factory (DriverFactory.cs)**: Seamlessly switches between `ChromeDriver` and `RemoteWebDriver` based on runtime configuration, applying `--no-sandbox`, `--disable-dev-shm-usage`, and `--disable-gpu` to stabilize Linux container execution.
* **Environment Variable Overrides (ConfigReader.cs)**: Leverages `Microsoft.Extensions.Configuration.EnvironmentVariables` to allow Docker environment flags (`TestSettings__UseGrid=true`) to override file settings non-invasively.
* **Cross-Platform Path Sanitization (TestLogger.cs)**: Uses platform-agnostic directory resolution (`Path.Combine`) to eliminate OS-specific path parsing failures inside Linux containers.
* **Shared Memory Allocation (shm_size: 2gb)**: Allocates sufficient shared memory to `selenium-chrome` in Docker Compose to prevent browser crashes during DOM-heavy operations.
* **SafeClick & Wait Strategies**: Custom wrappers absorb `ElementClickInterceptedException` and `StaleElementReferenceException` during single-page application (SPA) transitions.
* **BoDi Dependency Injection**: Scenario-scoped instance registration in `Hooks.cs` guarantees strict driver isolation and parallel-run stability.