using HtmlAgilityPack;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace BinaryIdiot
{
    public class UrlPreview
    {
        #region OG Constants

        private const string OGTitle = "og:title";
        private const string OGDescription = "og:description";
        private const string OGUrl = "og:url";
        private const string OGPublishDate = "og:pubdate";
        private const string OGSiteName = "og:site_name";
        private const string OGType = "og:type";
        private const string OGImage = "og:image";
        private const string OGImageWidth = "og:image:width";
        private const string OGImageHeight = "og:image:height";

        #endregion

        #region Public Properties

        public string BotName { get; set; } = "URLPreviewBot";
        public string BotVersion { get; set; } = "1.0";

        #endregion

        #region Private Methods
        private UrlPreviewModel _ProcessHtmlDocument(HtmlDocument document)
        {
            UrlPreviewModel result = new UrlPreviewModel();
            var metadata = document.DocumentNode.SelectNodes("//meta");
            if (metadata != null && metadata.Count > 0)
            {
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
            } else
            {
                return null;
            }

            return result;
        }

        private async Task<UrlPreviewModel> _FetchPage(Uri uri)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", String.Format("Mozilla/5.0 (compatible; {0}/{1})", this.BotName, this.BotVersion));
                using (HttpResponseMessage response = await client.GetAsync(uri))
                {
                    using (Stream stream = await response.Content.ReadAsStreamAsync())
                    {
                        HtmlDocument document = new HtmlDocument();
                        document.Load(stream);
                        return _ProcessHtmlDocument(document);
                    }
                }
            }
        }
        #endregion

        #region Public Methods
        public async Task<UrlPreviewModel> FetchPreviewFromUrlAsync(string url)
        {
            if (url == null || String.IsNullOrWhiteSpace(url))
            {
                return null;
            }

            try
            {
                Uri uri = new Uri(url);
                return await _FetchPage(uri);
            } catch (Exception)
            {
                throw new Exception(String.Format("The URL `{0}` supplied to UrlPreview.FetchPreviewFromUrlAsync() is not a valid URI", url));
            }
        }

        public async Task<UrlPreviewModel> FetchPreviewFromUrlAsync(Uri uri)
        {
            if (uri == null)
            {
                return null;
            }

            return await _FetchPage(uri);
        }

        public UrlPreviewModel FetchPreviewFromHtml(string html)
        {
            if (html == null)
            {
                return null;
            }

            HtmlDocument document = new HtmlDocument();
            using (Stream stream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(html);
                    writer.Flush();

                    document.Load(stream);

                    return _ProcessHtmlDocument(document);
                }
            }
        }

        public UrlPreviewModel FetchPreviewFromHtml(HtmlDocument document)
        {
            if (document == null)
            {
                return null;
            }

            return _ProcessHtmlDocument(document);
        }
        #endregion
    }
}
