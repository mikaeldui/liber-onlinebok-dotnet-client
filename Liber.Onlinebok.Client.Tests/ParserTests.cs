using Liber.Onlinebok.Client.Tests.SampleData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Liber.Onlinebok.Client.Tests
{
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public async Task TestDocumentAsync()
        {
            var sampleJson = await SampleDataHelper.GetStringAsync("document.json");

            var document = await LiberOnlinebokParser.ParseDocumentAsync(sampleJson);

            Assert.IsTrue(document.Content.ContentItems.Length > 0, "No content items.");

            Assert.IsNotNull(document.Content.ContentItems[0].Uuid, "Content items are empty.");

            Assert.IsTrue(document.Structure.Root.Children.Length > 0, "No structure children.");

            Assert.IsNotNull(document.Structure.Root.Children[0].Uuid, "Structure children are empty.");
        }

        [TestMethod]
        public async Task TestDocumentAttachmentsAsync()
        {
            var sampleJson = await SampleDataHelper.GetStringAsync("documentAttachments.json");

            var attachments = await LiberOnlinebokParser.ParseDocumentAttachmentsAsync(sampleJson);

            Assert.IsTrue(attachments.Length > 0, "No attachments.");

            Assert.IsNotNull(attachments[0].Uuid, "Attachments are empty.");
        }

        [TestMethod]
        public async Task TestAssetsLocationAsync()
        {
            var sampleJson = await SampleDataHelper.GetStringAsync("assetsLocation.json");

            var assetsLocation = await LiberOnlinebokParser.ParseAssertsLocationAsync(sampleJson);

            Assert.IsNotNull(assetsLocation, "Assets location is null.");
        }
    }
}
