using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LambdaSkill.Models
{
    class ClubSnippet
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "leaguePosition")]
        public string LeaguePosition { get; set; }
        [JsonProperty(PropertyName = "leaguePoints")]
        public string LeaguePoints { get; set; }
    }
}
