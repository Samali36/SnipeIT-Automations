# Snipe IT App QA Automation

Playwright .NET test automation project using Page Object Model.

## Project Structure

```
Global360/
├── Constants/           # Test constants and URLs
├── PageObjects/         # Page Object Model classes
├── TestData/           # Test data models
├── Tests/              # Test classes
├── Global360.csproj    # Project file
└── MSTestSettings.cs   # MSTest configuration
```

## How to Run

### Setup
```bash
# Install dependencies
dotnet restore
dotnet build

# Install Playwright browsers
pwsh bin/Debug/net8.0/playwright.ps1 install
```

### Run Tests
```bash
# Run all tests
dotnet test
```
