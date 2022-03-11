using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace Getters.Roll20;

public class Roll20Getter
{
    const string Roll20BaseUrl = "https://app.roll20.net";
    const string SearchPageUrlFormatString = Roll20BaseUrl + "/lfg/search/?yesmaturecontent=true&sortby=startingsoon&page=";
    const int MaxPagesToScrape = 200;
    private HttpClient httpClient;

    public Roll20Getter(IHttpClientFactory httpClientFactory)
    {
        httpClient = httpClientFactory.CreateClient();
    }

    [FunctionName("Roll20Getter")]
    public async Task Run([TimerTrigger("%Roll20GetterScheduleExpression%"
#if DEBUG
        , RunOnStartup = true
#endif
        )]TimerInfo myTimer,
        [CosmosDB(databaseName: "lookingforgroup3", containerName: "Roll20GamePages", Connection = "CosmosDBConnection")] IAsyncCollector<GameDetailsPage> gameDetailsPages,
        ILogger log)
    {
        log.LogInformation("Running Getter");
        for (var pageNumber = 0; pageNumber < MaxPagesToScrape; pageNumber++)
        {
            var searchResultPageRawHtml = await FetchSearchResultPage(pageNumber);
            if (!HasResults(searchResultPageRawHtml)) break;

            var gamePageDetailsUrls = GetGamePageDetailsUrls(searchResultPageRawHtml);
            foreach (var gamePageDetailsUrl in gamePageDetailsUrls)
            {
                var gameDetailsPage = await FetchGameDetailsPage(gamePageDetailsUrl);
                await gameDetailsPages.AddAsync(gameDetailsPage);
                return;
            }
        }
        log.LogInformation("Finished Getter");
    }

    private async Task<GameDetailsPage> FetchGameDetailsPage(string gamePageDetailsUrl)
    {
        var rawHtml = await httpClient.GetStringAsync(gamePageDetailsUrl);
        var id = gamePageDetailsUrl.Replace("https://app.roll20.net/lfg/listing/", "").Split('/')[0];
        return new GameDetailsPage { Id = id, Source = gamePageDetailsUrl, RawHtml = rawHtml, Timestamp = DateTimeOffset.UtcNow };
    }

    private IEnumerable<string> GetGamePageDetailsUrls(string searchResultPageRawHtml)
    {
        var doc = new HtmlAgilityPack.HtmlDocument();
        doc.LoadHtml(searchResultPageRawHtml);
        return doc.DocumentNode.SelectNodes("//a[@class='lfglistingname']").Select(a => Roll20BaseUrl + a.Attributes["href"].Value);
    }

    private async Task<string> FetchSearchResultPage(int pageNumber)
    {
        var urlToRequest = SearchPageUrlFormatString + pageNumber;
        var pageHtml = await httpClient.GetStringAsync(urlToRequest);
        return pageHtml;
    }

    bool HasResults(string rawHtml) => !rawHtml.Contains("Your search did not return any results");
}