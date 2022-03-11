using System.Collections.Generic;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Getters
{
    public static class Roll20GamePageParser
    {
        [FunctionName("Roll20GamePageParser")]
        public static void Run([CosmosDBTrigger(databaseName: "lookingforgroup3", containerName: "Roll20GamePages", 
            Connection = "CosmosDBConnection", CreateLeaseContainerIfNotExists = true)]
            IReadOnlyList<GameDetailsPage> input,
            ILogger log)
        {
            foreach (var gameDetailsPage in input)
            {
                log.LogInformation("Parsing {Id}");
            }
        }
    }
}