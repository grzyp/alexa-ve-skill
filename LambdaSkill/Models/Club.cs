using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LambdaSkill.Models
{
    class Club : Top3Club
    {
        [JsonProperty(PropertyName = "league")]
        public string League { get; set; }
    }
}
