# UrlPreview.net
A .Net Standard library for fetching enough information about a web page to provide a preview either through a URL or the raw HTML itself.

## How to use
It's pretty simple! Here's a quick example:

```c#
// Fetch preview from a url
var urlPreview = new UrlPreview();
var cnnResult = await urlPreview.FetchPreviewFromUrlAsync("https://www.cnn.com");
System.Console.WriteLine(cnnResult.Title);

// Fetch a preview from Html
var urlPreview = new UrlPreview();
var htmlResult = urlPreview.FetchPreviewFromHtml(myHtml);
System.Console.WriteLine(htmlResult.Title);
```

## Bot identification
Many sites require some sort of bot identification or it will fail. By default we supply a Mozilla compatible User Agent string with a bot name of `URLPreviewBot` and version of `1.0` but you can customize this!

```c#
// Simply provide values for the 'BotName' and 'BotVersion' properties
var urlPreview = new UrlPreview
{
    BotName = "CoolBot",
    BotVersion = "2.0"
};
```

## Roadmap
This library is still very young and plenty of work is planned. Have suggestions for feedback? Please feel free to submit GitHub issues, Pull Requests or just contact me directly @KrisSiegel.