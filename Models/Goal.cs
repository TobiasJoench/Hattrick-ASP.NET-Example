using System;
using System.Collections.Generic;

namespace HTStats.Models
{
    public partial class Goal
    {
        public int MatchId { get; set; }
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }
        public int PlayerId { get; set; }
        public byte AwayGoals { get; set; }
        public byte HomeGoals { get; set; }
        public byte Minute { get; set; }
        public byte MatchPart { get; set; }
    }
}
