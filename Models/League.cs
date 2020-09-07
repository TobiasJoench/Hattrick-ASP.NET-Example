using System;
using System.Collections.Generic;

namespace HTStats.Models
{
    public partial class League
    {
        public int SeriesId { get; set; }
        public int LeagueId { get; set; }
        public byte LeagueLevel { get; set; }
    }
}
