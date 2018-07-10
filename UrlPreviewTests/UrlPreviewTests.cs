using HtmlAgilityPack;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace BinaryIdiot
{
    public class UrlPreviewTests
    {
        [Fact]
        public void UrlPreviewConstructorSuccess()
        {
            var urlPreview = new UrlPreview();
            Assert.NotNull(urlPreview);
        }

        [Fact]
        public async Task FetchPreviewFromUrlAsync_FailsOnInvalidUrl()
        {
            var urlPreview = new UrlPreview();
            var task1 = await urlPreview.FetchPreviewFromUrlAsync("");
            Assert.Null(task1);

            var task2 = await urlPreview.FetchPreviewFromUrlAsync(null as string);
            Assert.Null(task2);

            var task3 = await urlPreview.FetchPreviewFromUrlAsync(null as Uri);
            Assert.Null(task3);

            await Assert.ThrowsAsync<Exception>(() => urlPreview.FetchPreviewFromUrlAsync("this is not a url lol;;;$$$$#$"));
        }

        [Fact]
        public void FetchPreviewFromHtml_FailsOnInvalidData()
        {
            var urlPreview = new UrlPreview();

            var result1 = urlPreview.FetchPreviewFromHtml(null as string);
            Assert.Null(result1);

            var result2 = urlPreview.FetchPreviewFromHtml(null as HtmlDocument);
            Assert.Null(result2);
        }

        [Fact]
        public async Task FetchPreviewFromUrlAsync_SuccessOnMultipleUrls()
        {
            var urlPreview = new UrlPreview();
            var task1 = await urlPreview.FetchPreviewFromUrlAsync("https://www.cnn.com");
            Assert.NotNull(task1);
            Assert.NotNull(task1.Title);
            Assert.NotNull(task1.Description);

            var task2 = await urlPreview.FetchPreviewFromUrlAsync("https://www.foxnews.com");
            Assert.NotNull(task2);
            Assert.NotNull(task2.Title);
            Assert.NotNull(task2.Description);

            var task3 = await urlPreview.FetchPreviewFromUrlAsync("https://www.bloomberg.com");
            Assert.NotNull(task3);
            Assert.NotNull(task3.Title);
            Assert.NotNull(task3.Description);

            var task4 = await urlPreview.FetchPreviewFromUrlAsync("https://techcrunch.com");
            Assert.NotNull(task4);
            Assert.NotNull(task4.Title);
            Assert.NotNull(task4.Description);
        }

        [Fact]
        public void FetchPreviewFromHtml_SuccessOnMultipleFiles()
        {
            var urlPreview = new UrlPreview();
            
            const string fileName1 = "./Resources/CNN - Tapping into craft beer's online potential.html";
            using (var fileStream = new FileStream(fileName1, FileMode.Open))
            {
                using (var reader = new StreamReader(fileStream))
                {
                    var result = urlPreview.FetchPreviewFromHtml(reader.ReadToEnd());
                    Assert.NotNull(result);
                }
            }

            const string fileName2 = "Fox Business - Stocks jump on final trading day of 2Q.html";
            const string fileName3 = "Twitter - A Microsoft Tweet.html";
            const string fileName4 = "Wikipedia - Hayabusa2.html";
        }
    }
}
