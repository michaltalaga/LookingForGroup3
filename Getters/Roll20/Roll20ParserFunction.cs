using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Getters.Roll20;
public class Roll20ParserFunction
{
    private readonly IRoll20GamePageParser roll20GamePageParser;

    public Roll20ParserFunction(IRoll20GamePageParser roll20GamePageParser)
    {
        this.roll20GamePageParser = roll20GamePageParser;
    }

    [FunctionName("Roll20GamePageParser")]
    public async Task Run(
        [CosmosDBTrigger(databaseName: "lookingforgroup3", containerName: "Roll20GamePages", Connection = "CosmosDBConnection")] IReadOnlyList<GameDetailsScrappedPage> gameDetailsPages,
        [CosmosDB(databaseName: "lookingforgroup3", containerName: "Roll20Games", Connection = "CosmosDBConnection")] IAsyncCollector<Game> games,
        ILogger log)
    {
        foreach (var gameDetailsPage in gameDetailsPages)
        {
            log.LogInformation("Parsing {Id}", gameDetailsPage.Id);
            var game = roll20GamePageParser.Parse(gameDetailsPage);
            await games.AddAsync(game);
            log.LogInformation("Parsed {Id}", game.Id);
        }
    }
}