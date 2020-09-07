using System;
using System.Collections.Generic;

namespace HTStats.Models
{
    public partial class MatchRating
    {
        public int MatchId { get; set; }
        public int TeamId { get; set; }
        public string Formation { get; set; }
        public byte Goals { get; set; }
        public byte GoalsAgainst { get; set; }
        public byte TacticType { get; set; }
        public byte TacticSkill { get; set; }
        public byte RatingMidfield { get; set; }
        public byte RatingRightDef { get; set; }
        public byte RatingMidDef { get; set; }
        public byte RatingLeftDef { get; set; }
        public byte RatingRightAtt { get; set; }
        public byte RatingMidAtt { get; set; }
        public byte RatingLeftAtt { get; set; }
        public byte TeamAttitude { get; set; }
        public byte? RatingIndirectSpDef { get; set; }
        public byte? RatingIndirectSpAtt { get; set; }
        public byte BpFirst { get; set; }
        public byte BpSecond { get; set; }
        public byte Result { get; set; }
    }
}
