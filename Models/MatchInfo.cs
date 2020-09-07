using System;
using System.Collections.Generic;

namespace HTStats.Models
{
    public partial class MatchInfo
    {
        public int MatchId { get; set; }
        public byte MatchType { get; set; }
        public int MatchContextId { get; set; }
        public byte MatchRuleId { get; set; }
        public int CupLevel { get; set; }
        public byte CupLevelIndex { get; set; }
        public DateTime MatchDate { get; set; }
        public byte AddedMinutes { get; set; }
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }
        public byte WeatherId { get; set; }
        public byte HomeGoals { get; set; }
        public byte AwayGoals { get; set; }
    }
}
