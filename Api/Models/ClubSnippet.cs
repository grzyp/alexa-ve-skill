using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
    public class ClubSnippet
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "leaguePosition")]
        public string LeaguePosition { get; set; }
        [JsonProperty(PropertyName = "leaguePoints")]
        public string LeaguePoints { get; set; }
    }
}
