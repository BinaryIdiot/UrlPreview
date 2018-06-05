using Microsoft.VisualStudio.TestTools.UnitTesting;
using BinaryIdiot;
using System.Threading.Tasks;

namespace BinaryIdiot
{
    [TestClass]
    public class UrlPreviewTests
    {
        [TestMethod]
        public async Task FetchAndParseCNN()
        {
            UrlPreviewModel result = await UrlPreview.FetchPage("https://www.cnn.com/");
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Title);
            Assert.IsNotNull(result.Description);
        }
    }
}
