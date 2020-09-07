using System;
using System.Collections.Generic;

namespace HTStats.Models
{
    public partial class LeagueInfo
    {
        public int LeagueId { get; set; }
        public string LeagueName { get; set; }
        public byte MaxLeagueLevel { get; set; }
        public int Season { get; set; }
    }
}
