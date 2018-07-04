using HtmlAgilityPack;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BinaryIdiot
{
    public class UrlPreview
    {
        private const string OGTitle = "og:title";
        private const string OGDescription = "og:description";
        private const string OGUrl = "og:url";
        private const string OGPublishDate = "og:pubdate";
        private const string OGSiteName = "og:site_name";
        private const string OGType = "og:type";
        private const string OGImage = "og:image";
        private const string OGImageWidth = "og:image:width";
        private const string OGImageHeight = "og:image:height";

        private static UrlPreviewModel _ProcessHtmlDocument(string raw)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(raw)))
            {
                return _ProcessHtmlDocument(stream);
            }
        }

        private static UrlPreviewModel _ProcessHtmlDocument(Stream stream)
        {
            UrlPreviewModel result = new UrlPreviewModel();
            HtmlDocument document = new HtmlDocument();
            document.Load(stream);

            var metadata = document.DocumentNode.SelectNodes("//meta");
            foreach (var node in metadata)
            {
                string property = node.GetAttributeValue("property", "");
                string content = node.GetAttributeValue("content", "");

                switch (property)
                {
                    case OGTitle:
                        result.Title = content;
                        break;
                    case OGDescription:
                        result.Description = content;
                        break;
                    case OGUrl:
                        result.Url = content;
                        break;
                    case OGPublishDate:
                        result.PublishDate = DateTime.Parse(content);
                        break;
                    case OGSiteName:
                        result.SiteName = content;
                        break;
                    case OGType:
                        result.Type = content;
                        break;
                    case OGImage:
                        result.ImageUrl = content;
                        break;
                    case OGImageWidth:
                        result.ImageWidth = content;
                        break;
                    case OGImageHeight:
                        result.ImageHeight = content;
                        break;
                    default:
                        // Do nothing
                        break;
                }
            }

            return result;
        }

        private static async Task<UrlPreviewModel> _FetchPage(Uri uri)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(uri))
                {
                    using (Stream stream = await response.Content.ReadAsStreamAsync())
                    {
                        return _ProcessHtmlDocument(stream);
                    }
                }
            }
        }

        public static UrlPreviewModel FetchPreviewFromHtml(string html)
        {
            if (html == null || String.IsNullOrWhiteSpace(html))
            {
                return null;
            }

            return _ProcessHtmlDocument(html);
        }

        public static async Task<UrlPreviewModel> FetchPreviewFromUrlAsync(string url)
        {
            if (url == null || String.IsNullOrWhiteSpace(url))
            {
                return null;
            }

            Uri uri = new Uri(url);

            return await _FetchPage(uri);
        }

        public static async Task<UrlPreviewModel> FetchPreviewFromUrlAsync(Uri uri)
        {
            if (uri == null)
            {
                return null;
            }

            return await _FetchPage(uri);
        }
    }
}
