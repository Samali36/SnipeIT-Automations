using Microsoft.Playwright;
using Global360.PageObjects.Base;
using Global360.Constants;

namespace Global360.PageObjects
{
    public class DashboardPage : BasePage
    {
        // Selectors
        private readonly string _dashboardTitle = "h1";
        private readonly string _assetsMenuLink = "a[href*='/hardware']";
        private readonly string _createAssetButton = ".btn:has-text('Create Asset')";


        public DashboardPage(IPage page) : base(page) { }

        public async Task<bool> IsOnDashboardAsync()
        {
            return await IsVisibleAsync(_dashboardTitle);
        }

       public async Task NavigateToAssetsAsync()
        {
            await ClickAsync(_assetsMenuLink);
            await WaitForLoadingToDisappearAsync();
        }

        public async Task ClickCreateAssetAsync()
        {
            if (await IsVisibleAsync(_createAssetButton))
            {
                await ClickAsync(_createAssetButton);
            }
        }

    }
}