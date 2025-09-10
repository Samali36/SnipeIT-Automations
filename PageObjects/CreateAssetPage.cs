using Microsoft.Playwright;
using Global360.PageObjects.Base;
using Global360.Constants;
using Global360.TestData;

namespace SnipeITTestProject.PageObjects
{
    public class CreateAssetPage : BasePage
    {
        private readonly string _assetTagField = "input[id='asset_tag']";
        private readonly string _modelDropdownSpan = "#model_select_id + span .select2-selection";
        private readonly string _dropdownOptions = ".select2-results__option";
        private readonly string _statusDropdown = "select[id='status_select_id']";
        private readonly string _assignedUserSpan = "#assigned_user_select + span .select2-selection";
        private readonly string _saveButton = ".btn-primary:has-text('Save')";
        private readonly string _successMessage = ".alert-success";

        public CreateAssetPage(IPage page) : base(page) { }

        public async Task NavigateAsync()
        {
            await NavigateToAsync(TestConstants.CreateAssetPage);
        }

        public async Task CreateAssetAsync(AssetData assetData)
        {
            // Fill asset tag
            await FillAsync(_assetTagField, assetData.AssetTag);
            
            // Select model - try different approaches
            await SelectModelAsync(assetData.Model);

            // Select status
            await SelectStatusAsync(assetData.Status);

            // Select random assigned user.
            await SelectAssignedUserAsync();

            // Save the asset
            await ClickAsync(_saveButton);
            await WaitForLoadingToDisappearAsync();
        }

       private async Task SelectModelAsync(string modelName)
        {
            await ClickAsync(_modelDropdownSpan);
            await ClickDropDownAsync(_dropdownOptions, modelName);
        }

        private async Task SelectStatusAsync(string statusName)
        {
            await SelectDropdownByTextAsync(_statusDropdown, statusName);
        }

        private async Task SelectAssignedUserAsync()
        {
            await _page.WaitForTimeoutAsync(3000);
            await SelectRandomDropDownOptionAsync(_assignedUserSpan, _dropdownOptions);

        }

        public async Task<bool> IsAssetCreatedSuccessfullyAsync()
        {
            return await IsVisibleAsync(_successMessage);
        }

        public async Task<string> GetSuccessMessageAsync()
        {
            return await GetTextAsync(_successMessage);
        }
    }
}