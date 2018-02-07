using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
    public class LeagueConfig
    {
        public string SearchURL { get; set; }
        public List<string> Men { get; set; }
        public List<string> Women { get; set; }
    }
}
