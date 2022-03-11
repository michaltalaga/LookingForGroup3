using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Getters
{
    public class GameDetailsPage
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        public string RawHtml { get; set; }
        public string Source { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}