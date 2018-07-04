using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace BinaryIdiot
{
    [TestClass]
    public class UrlPreviewTests
    {
        [TestMethod]
        public async Task FetchAndParseCNNFromUrl()
        {
            UrlPreviewModel result = await UrlPreview.FetchPreviewFromUrlAsync("https://www.cnn.com/");
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Title);
            Assert.IsNotNull(result.Description);
        }
    }
}
