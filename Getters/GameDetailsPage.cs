using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Getters
{
    public class GameDetailsPage
    {
        public string ExternalId { get; set; }
        public string RawHtml { get; internal set; }
        public string Source { get; internal set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}