using Xunit;
using System.Threading.Tasks;

namespace BinaryIdiot
{
    public class UrlPreviewTests
    {
        [Fact]
        public void FetchAndParseCNNFromUrl()
        {
            var task = UrlPreview.FetchPreviewFromUrlAsync("https://www.cnn.com");
            task.Wait();
            var result = task.Result;
            Assert.NotNull(result);
            Assert.NotNull(result.Title);
            Assert.NotNull(result.Description);
        }

        [Fact]
        public void FetchAndParseBloombergFromUrl()
        {
            var task = UrlPreview.FetchPreviewFromUrlAsync("https://www.bloomberg.com/news/articles/2018-07-04/poland-s-isolation-shown-as-premier-is-slammed-in-eu-parliament");
            task.Wait();
            var result = task.Result;
            Assert.NotNull(result);
            //Assert.NotNull(result.Title);
            //Assert.NotNull(result.Description);
        }
    }
}
