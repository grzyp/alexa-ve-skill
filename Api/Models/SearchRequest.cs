using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
    public class SearchRequest
    {
        [JsonProperty(PropertyName = "searchPhrase")]
        public string SearchPhrase { get; set; }
        [JsonProperty(PropertyName = "isMen")]
        public bool IsMen { get; set; }
    }
}
