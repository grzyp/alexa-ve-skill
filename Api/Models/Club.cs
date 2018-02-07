using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
    public class Club : Top3Club
    {
        [JsonProperty(PropertyName = "league")]
        public string League { get; set; }
    }
}
