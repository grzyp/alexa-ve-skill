using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LambdaSkill.Models
{
    class League
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "top3Clubs")]
        public List<Top3Club> Top3Clubs { get; set; }
        [JsonProperty(PropertyName = "otherCLubs")]
        public List<ClubSnippet> OtherClubs { get; set; }
    }
}
