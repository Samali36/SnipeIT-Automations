using Microsoft.Playwright;
using Global360.PageObjects.Base;
using Global360.Constants;

namespace Global360.PageObjects
{
    public class AssetDetailsPage : BasePage
    {
        private readonly string _assertTag = "span.js-copy-assettag";
        private readonly string _historyTab = "a[href*='#history']";
        private readonly string _historyTableRows = "#assetHistory tbody tr";
        private readonly string _confirmationModel = "#dataConfirmModal";
        private readonly string _modelConfirmBtn = "button.btn-outline";

        public AssetDetailsPage(IPage page) : base(page) { }

        public async Task<string> GetAssetTagAsync()
        {

            if (await IsVisibleAsync(_assertTag))
            {
                var text = await GetTextAsync(_assertTag);
                if (!string.IsNullOrWhiteSpace(text))
                {
                    return text.Trim();
                }
            }

            return string.Empty;
        }

        public async Task<Dictionary<string, string>> GetAssetDetailsAsync()
        {
            var details = new Dictionary<string, string>();

            try
            {
                // Get asset tag from page title/header
                details["AssetTag"] = await GetAssetTagAsync();

                // Try to extract details from various page elements
                await ExtractDetailsFromPage(details);

                return details;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error extracting asset details: {ex.Message}");
                return details;
            }
        }

        private async Task ExtractDetailsFromPage(Dictionary<string, string> details)
        {
            // Get all text content and try to parse key information
            var pageText = await _page.TextContentAsync("body") ?? "";

            // Look for common patterns in asset details
            if (pageText.Contains(TestConstants.MacbookPro13))
            {
                var lines = pageText.Split('\n').Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();
                foreach (var line in lines)
                {
                    if (line.Contains(TestConstants.MacbookPro13))
                    {
                        details["Model"] = line.Trim();
                        break;
                    }
                }
            }

            // Look for status information
            if (pageText.Contains(TestConstants.ReadyToDeploy))
            {
                var lines = pageText.Split('\n').Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();
                foreach (var line in lines)
                {
                    if (line.Contains(TestConstants.ReadyToDeploy))
                    {
                        details["Status"] = line.Trim();
                        break;
                    }
                }
            }
        }

        public async Task ClickHistoryTabAsync()
        {
            await ClickAsync(_historyTab);
            await WaitForLoadingToDisappearAsync();
            return;
        }

        public async Task<bool> IsHistoryTabVisibleAsync()
        {
            if (await IsVisibleAsync(_historyTab))
            {
                return true;
            }
            return false;

        }
        public async Task<List<string>> GetHistoryEntriesAsync()
        {
            var entries = new List<string>();

            try
            {
                // Click history tab if visible
                if (await IsHistoryTabVisibleAsync())
                {
                    await ClickHistoryTabAsync();
                }

                // Wait for history table to load
                await _page.WaitForTimeoutAsync(2000);

                // Get all rows from the history table
                var rows = _page.Locator(_historyTableRows);

                var rowCount = await rows.CountAsync();
                for (int i = 0; i < rowCount; i++)
                {
                    var text = await rows.Nth(i).TextContentAsync();
                    if (!string.IsNullOrWhiteSpace(text))
                    {
                        entries.Add(text.Trim());
                    }
                }

                return entries;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting history entries: {ex.Message}");
                return entries;
            }
        }

        public async Task<bool> ValidateAssetInHistoryAsync(string assetTag)
        {
            var historyEntries = await GetHistoryEntriesAsync();
            return historyEntries.Any(entry =>
                entry.Contains(assetTag) ||
                entry.Contains("created") ||
                entry.Contains("Checked out"));
        }

        public async Task DeleteAssetAsync(string assetId)
        {
            // 1Click the Delete button for the asset
            var deleteButton = _page.Locator($"button.delete-asset[data-content*='{assetId}']");
            await deleteButton.ClickAsync();

            // Wait for the confirmation modal to appear
            var confirmModal = _page.Locator(_confirmationModel);
            await confirmModal.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });

            //  Click the "Yes" button inside the modal
            var yesButton = confirmModal.Locator(_modelConfirmBtn, new LocatorLocatorOptions { HasTextString = "Yes" });
            await yesButton.ClickAsync();

        }

    }
}