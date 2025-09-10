using Microsoft.Playwright;
using Global360.PageObjects;
using Global360.Constants;
using SnipeITTestProject.PageObjects;

namespace Global360.Tests.Base
{
    [TestClass]
    public abstract class BaseTest
    {
        protected IPlaywright Playwright { get; private set; } = null!;
        protected IBrowser Browser { get; private set; } = null!;
        protected IBrowserContext Context { get; private set; } = null!;
        protected IPage Page { get; private set; } = null!;

        // Page Objects
        protected LoginPage LoginPage { get; private set; } = null!;
        protected DashboardPage DashboardPage { get; private set; } = null!;
        protected AssetsListPage AssetsListPage { get; private set; } = null!;
        protected CreateAssetPage CreateAssetPage { get; private set; } = null!;
        protected AssetDetailsPage AssetDetailsPage { get; private set; } = null!;

        [TestInitialize]
        public async Task SetUpAsync()
        {
            // Create Playwright instance
            Playwright = await Microsoft.Playwright.Playwright.CreateAsync();

            Browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false, // Head mode
            });

            // Default context and page
            Context = await Browser.NewContextAsync();
            Page = await Context.NewPageAsync();

            // Initialize page objects
            LoginPage = new LoginPage(Page);
            DashboardPage = new DashboardPage(Page);
            AssetsListPage = new AssetsListPage(Page);
            CreateAssetPage = new CreateAssetPage(Page);
            AssetDetailsPage = new AssetDetailsPage(Page);
            
        }

        [TestCleanup]
        public async Task TearDownAsync()
        {
            await Context.CloseAsync();
            await Browser.CloseAsync();
            Playwright.Dispose();
        }

        protected async Task LoginToDemoAsync()
        {
            await LoginPage.NavigateAsync();
            await LoginPage.LoginAsync(
                TestConstants.UserName,
                TestConstants.Password);

            Assert.IsTrue(await LoginPage.IsLoginSuccessfulAsync(), "Login should be successful");
        }
    }
}
