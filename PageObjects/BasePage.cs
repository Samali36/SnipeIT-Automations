using Microsoft.Playwright;
using Global360.Constants;

namespace Global360.PageObjects.Base
{
    public abstract class BasePage
    {
        protected readonly IPage _page;

        protected BasePage(IPage page)
        {
            _page = page;
        }

        // Common actions
        public async Task NavigateToAsync(string url)
        {
            await _page.GotoAsync(TestConstants.BaseUrl + url);
        }

        public async Task ClickAsync(string selector)
        {
            await _page.ClickAsync(selector, new PageClickOptions { Timeout = TestConstants.DefaultTimeout });
        }

        public async Task ClickDropDownAsync(string selector, string value)
        {
            await _page.Locator(selector, new PageLocatorOptions { HasTextString = value }).ClickAsync();
        }
        public async Task SelectRandomDropDownOptionAsync(string dropdownSpanSelector, string optionsSelector)
        {
            // 1️⃣ Click the dropdown to open it
            var select2 = _page.Locator(dropdownSpanSelector);
            await select2.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
            await select2.ClickAsync();

            // 2️⃣ Wait for options to appear
            var optionLocator = _page.Locator($"{optionsSelector}:not(.loading-results)");
            await optionLocator.First.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });

            // 3️⃣ Get all option texts
            var options = await optionLocator.AllTextContentsAsync();
            if (!options.Any())
                throw new Exception("No selectable options found in the dropdown.");

            // 4️⃣ Pick a random option
            var random = new Random();
            var index = options.Count > 1 ? random.Next(0, options.Count) : 0;

            // 5️⃣ Click the exact option using Nth() to satisfy strict mode
            await optionLocator.Nth(index).ClickAsync();

            Console.WriteLine($"Selected random option: {options[index]}");
        }

        public async Task FillAsync(string selector, string value)
        {
            await _page.FillAsync(selector, value, new PageFillOptions { Timeout = TestConstants.DefaultTimeout });
        }

        public async Task<string> GetTextAsync(string selector)
        {
            return await _page.TextContentAsync(selector, new PageTextContentOptions { Timeout = TestConstants.DefaultTimeout }) ?? string.Empty;
        }

        public async Task<bool> IsVisibleAsync(string selector)
        {
            return await _page.IsVisibleAsync(selector, new PageIsVisibleOptions { });
        }

        public async Task WaitForSelectorAsync(string selector)
        {
            await _page.WaitForSelectorAsync(selector, new PageWaitForSelectorOptions { Timeout = TestConstants.DefaultTimeout });
        }

        public async Task SelectDropdownByTextAsync(string selector, string optionText)
        {
            await _page.SelectOptionAsync(selector, new SelectOptionValue { Label = optionText });
        }


        public async Task WaitForLoadingToDisappearAsync(int timeoutMs = 30000)
        {
            await _page.WaitForTimeoutAsync(3000);
        }

    }
}