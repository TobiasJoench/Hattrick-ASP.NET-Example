using System;
using System.Collections.Generic;

namespace HTStats.Models
{
    public partial class Team
    {
        public int TeamId { get; set; }
        public int LeagueId { get; set; }
        public int SeriesId { get; set; }
        public DateTime FoundedDate { get; set; }
        public byte LeagueLevel { get; set; }
        public int CountryId { get; set; }
        public int Trainer { get; set; }
        public int GlobalRanking { get; set; }
        public int LeagueRanking { get; set; }
        public int PowerRanking { get; set; }
        public DateTime? Updated { get; set; }
    }
}
