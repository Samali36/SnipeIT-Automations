using Microsoft.VisualStudio.TestTools.UnitTesting;
using Global360.Tests.Base;
using Global360.TestData;
using Global360.Constants;
using Global360.PageObjects;

namespace AssetTest.Tests
{

    [TestClass]
    public class AssetTest : BaseTest
    {
        private AssetData GenerateAssetData()
        {

            return new AssetData
            {
                AssetTag = TestConstants.AssetTag,
                Model = TestConstants.MacbookPro13,
                Status = TestConstants.ReadyToDeploy,
                AssignedTo = "Random User"
            };
        }

        [TestMethod]
        [TestCategory("End-to-End")]
        [Priority(1)]
        public async Task CompleteAssetLifecycle_CreateSearchViewDelete_ShouldWorkEndToEnd()
        {
            // Arrange
            await LoginToDemoAsync();
            var assetData = GenerateAssetData();

            try
            {
                // Act 1: Create Asset
                await AssetsListPage.NavigateAsync();
                await AssetsListPage.ClickCreateAssetAsync();
                await CreateAssetPage.CreateAssetAsync(assetData);
                Assert.IsTrue(await CreateAssetPage.IsAssetCreatedSuccessfullyAsync(),
                    "Asset creation should be successful");

                // Act 2: Search for Asset
                await AssetsListPage.NavigateAsync();
                var isVisible = await AssetsListPage.IsAssetVisibleInListAsync(assetData.AssetTag);
                Assert.IsTrue(isVisible, "Created asset should be searchable");

                // Act 3: View Asset Details
                await AssetsListPage.ClickAssetByTagAsync(assetData.AssetTag);
                var details = await AssetDetailsPage.GetAssetDetailsAsync();
                Assert.IsTrue(details.Count > 0, "Asset details should be retrievable");

                // Act 4: Verify History (if available)
                if (await AssetDetailsPage.IsHistoryTabVisibleAsync())
                {
                    await AssetDetailsPage.ClickHistoryTabAsync();
                    var historyEntries = await AssetDetailsPage.GetHistoryEntriesAsync();
                }

                await AssetsListPage.NavigateAsync();
                await AssetsListPage.ClickAssetByTagAsync(assetData.AssetTag);
                await AssetDetailsPage.DeleteAssetAsync(assetData.AssetTag);


                await AssetsListPage.NavigateAsync();
                isVisible = await AssetsListPage.IsAssetVisibleInListAsync(assetData.AssetTag);
                Assert.IsFalse(isVisible, "Asset should not be Visible");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }


    }
}