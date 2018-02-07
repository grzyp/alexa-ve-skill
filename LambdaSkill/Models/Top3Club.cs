using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LambdaSkill.Models
{
    class Top3Club : ClubSnippet
    {
        [JsonProperty(PropertyName = "gamesPlayed")]
        public string GamesPlayed { get; set; }
        [JsonProperty(PropertyName = "gamesWon")]
        public string GamesWon { get; set; }
        [JsonProperty(PropertyName = "gamesLost")]
        public string GamesLost { get; set; }
    }
}
