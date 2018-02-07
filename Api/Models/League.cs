using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
    public class League
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "top3Clubs")]
        public List<Top3Club> Top3Clubs { get; set; }
        [JsonProperty(PropertyName = "otherCLubs")]
        public List<ClubSnippet> OtherClubs { get; set; }

        public League (string name)
        {
            Top3Clubs = new List<Top3Club>();
            OtherClubs = new List<ClubSnippet>();
            Name = name;
        }
    }
}
