using Microsoft.Playwright;
using Global360.PageObjects.Base;
using Global360.Constants;

namespace Global360.PageObjects
{
    public class AssetsListPage : BasePage
    {
        private readonly string _createAssetButton = "button[name='btnAdd']";
        private readonly string _searchBox = "input[type='search']";

        public AssetsListPage(IPage page) : base(page) { }

        public async Task NavigateAsync()
        {
            await NavigateToAsync(TestConstants.AssetsPage);
            await WaitForLoadingToDisappearAsync();
        }

        public async Task ClickCreateAssetAsync()
        {
            await ClickAsync(_createAssetButton);
        }

        public async Task SearchForAssetAsync(string assetTag)
        {
            await FillAsync(_searchBox, assetTag);
            await _page.Keyboard.PressAsync("Enter");
            await WaitForLoadingToDisappearAsync();
        }

        public async Task<bool> IsAssetVisibleInListAsync(string assetTag)
        {
            await SearchForAssetAsync(assetTag);
            var assetLink = $"a:has-text('{assetTag}')";
            return await IsVisibleAsync(assetLink);
        }

        public async Task ClickAssetByTagAsync(string assetTag)
        {
            await SearchForAssetAsync(assetTag);
            var assetLink = $"a:has-text('{assetTag}')";
            await WaitForSelectorAsync(assetLink);
            await ClickAsync(assetLink);
            await WaitForLoadingToDisappearAsync();
        }

        public async Task<Dictionary<string, string>> GetAssetDetailsFromListAsync(string assetTag)
        {
            await SearchForAssetAsync(assetTag);
            var details = new Dictionary<string, string>();
            
            // Wait for the asset row to appear
            var assetRowSelector = $"tr:has(a:text('{assetTag}'))";
            await WaitForSelectorAsync(assetRowSelector);
            
            var row = await _page.QuerySelectorAsync(assetRowSelector);
            if (row != null)
            {
                var cells = await row.QuerySelectorAllAsync("td");
                if (cells.Count > 0)
                {
                    details["AssetTag"] = await cells[0].TextContentAsync() ?? "";
                    details["Name"] = await cells[1].TextContentAsync() ?? "";
                    details["Model"] = cells.Count > 2 ? await cells[2].TextContentAsync() ?? "" : "";
                    details["Status"] = cells.Count > 3 ? await cells[3].TextContentAsync() ?? "" : "";
                }
            }
            
            return details;
        }
    }
}